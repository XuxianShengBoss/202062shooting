using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Newtonsoft.Json;

public class Excel2Unity
{
    private static string AssetsRootPath;
    private const string SCRIPT_PATH = "/Scripts/System/Database/";
    private const string SCRIPT_ENUM_PATH = SCRIPT_PATH + "Enum/";
    private const string SCRIPT_DATA_PATH = SCRIPT_PATH + "Data/";
    private const string DATA_PATH = "Assets/Resources/AssetsBundle/";
    private const string assetsPath = DATA_PATH + "Data.asset";
    private const string abPath = DATA_PATH + "DataAB.unity3d";

    static Excel2Unity()
    {
        AssetsRootPath = Application.dataPath;
        AssetsRootPath = AssetsRootPath.Substring(0, AssetsRootPath.LastIndexOf("/")) + "/";
    }

    [MenuItem("Excel/ParseEnum")]
    public static void ParseEnum()
    {
        LoadExcel();

        Dictionary<string, string> enumTable = ExcelReader.tableEnums;
        foreach (KeyValuePair<string, string> kv in enumTable)
        {
            WriteFile(kv.Key, kv.Value);
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Excel/ParseData")]
    public static void ParseData()
    {
        LoadExcel();
        //Debug.LogError (AssetsRootPath + DATA_PATH);
        if (!System.IO.Directory.Exists(AssetsRootPath + DATA_PATH))
        {
            System.IO.Directory.CreateDirectory(AssetsRootPath + DATA_PATH);
        }

        Dictionary<string, string> dataTable = ExcelReader.tableData;
        List<string[]> strTable = ExcelReader.tableStrs;

        GameDB db = ScriptableObject.CreateInstance<GameDB>();
        ParseGameData(db, dataTable, strTable);

        //		foreach (CarData c in db.cars) {
        //			Debug.LogError (c.id+"   "+c.name);
        //		}

        WriteAssetsBundle(db);

    }

    public static void LoadExcel()
    {

        object[] selection = (object[])Selection.objects;
        //判断是否有对象被选中
        if (selection.Length == 0)
        {
            Debug.LogError("请选择一个Excel文件");
            return;
        }
        //遍历每一个对象判断不是Excel文件
        ExcelReader.Clear();
        foreach (UnityEngine.Object obj in selection)
        {
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (objPath.EndsWith(".xlsx"))
            {
                //Debug.LogError (objPath);

                ExcelReader.ParseExcel(AssetsRootPath + "/" + objPath);
            }
        }
    }

    public static void ParseGameData(GameDB db, Dictionary<string, string> table, List<string[]> strTable)
    {
        Type type = typeof(GameDB);
        FieldInfo[] fields = type.GetFields();

        foreach (FieldInfo fi in fields)
        {
            if (fi.Name == "strTableCN")
            {
                fi.SetValue(db, strTable[0]);
            }
            else if(fi.Name == "strDataTableCN")
            {
                fi.SetValue(db, strTable[2]);
            }
            else if(fi.Name == "strTableEN")
            {
                fi.SetValue(db, strTable[1]);
            }
            else if (fi.Name == "strDataTableEN")
            {
                fi.SetValue(db, strTable[3]);
            }
            else
            {
                string name = fi.FieldType.GetGenericArguments()[0].ToString();
                string json;
                if (!table.TryGetValue(name, out json)) continue;

                Debug.Log("data name:" + name);
                //找到 T DeserializeObject<T> (string value) 这个方法
                MethodInfo[] mis = typeof(JsonConvert).GetMethods();//"DeserializeObject", new Type[]{typeof(string)});
                MethodInfo mi = null;
                foreach (MethodInfo m in mis)
                {
                    if (m.Name == "DeserializeObject" && m.IsGenericMethod && m.GetParameters().Length == 1)
                    {
                        mi = m;
                    }
                }
                if (mi != null)
                {
                    //				Debug.LogError(mi.IsGenericMethod);
                    MethodInfo gmi = mi.MakeGenericMethod(new Type[] { fi.FieldType });
                    object o = gmi.Invoke(null, new object[] { json });
                    fi.SetValue(db, o);
                }
            }


            //Debug.LogError(players.Count+"    PLAYERS");
        }
    }

    private static void WriteAssetsBundle(GameDB db)
    {
        //创建“资源文件”，可在inspector查看和编辑
        AssetDatabase.CreateAsset(db, assetsPath);

        //将“资源文件”创建成AssetBundle文件
        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(assetsPath, typeof(GameDB));

#if UNITY_ANDROID
		BuildTarget target = BuildTarget.Android;
#elif UNITY_IOS
		BuildTarget target = BuildTarget.iOS;
#else
        BuildTarget target = BuildTarget.StandaloneWindows64;
#endif
        BuildPipeline.BuildAssetBundle(o, null, abPath, BuildAssetBundleOptions.None, target);

        //如无需在inspector修改“资源文件”的数据，可删除
        //AssetDatabase.DeleteAsset(assetsPath);

        //刷新
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log("Build Assets Success!    (Data Path: " + abPath + ")");
    }

    private static void WriteFile(string fileName, string content)
    {

        string path = Application.dataPath + SCRIPT_ENUM_PATH;

        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        FileStream fs = new FileStream(path + fileName + ".cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

}