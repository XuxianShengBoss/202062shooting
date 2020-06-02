using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RootMotion;
using RootMotion.Dynamics;
using LevelData;
public class Help 
{
    [MenuItem("PuppetMaster/BipedRagdollCreator")]
    public static void Create()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("请选择编辑对象");
            return;
        }
        if(puppet==null)
        puppet=Resources.Load<GameObject>("PrefabPuppet").GetComponentInChildren<BehaviourPuppet>();
        bool isSelectionHir = Selection.activeGameObject.activeInHierarchy;
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject @object = null;
            if (!isSelectionHir)
                @object = GameObject.Instantiate<GameObject>(objects[i], objects[i].transform.position, objects[i].transform.rotation);
            else
            {
                @object = objects[i];
                if (UnityEditor.PrefabUtility.GetPrefabObject(@object))
                    PrefabUtility.UnpackPrefabInstance(@object, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            }
            @object.name = objects[i].name;
            CreatePuppeMaster(@object.transform);
        }
    }
    static BehaviourPuppet puppet;
    public static void CreatePuppeMaster(Transform transform)
    {
        var _aniobj = transform ;
        var r = BipedRagdollReferences.FromAvatar(_aniobj.transform.GetComponentInChildren<Animator>());
        var options = BipedRagdollCreator.AutodetectOptions(r);
        BipedRagdollCreator.Create(r, options);
       PuppetMaster master=  PuppetMaster.SetUp(_aniobj.transform, 8, 9);
       Transform Behavioursroot=master.transform.parent.Find("Behaviours");
        GameObject root= new GameObject("Behavioursroot");
        root.transform.SetParent(Behavioursroot);
			if(puppet!=null)
			{
			UnityEditorInternal.ComponentUtility.CopyComponent(puppet);
			UnityEditorInternal.ComponentUtility.PasteComponentAsNew(root.gameObject);
			}else
            DebugUtility.DebugLogError("检查模版===> BehaviourPuppet 是否存在");
    }

    #region //关卡数据写入
    [MenuItem("Tools/CreadWaveData")]
    public static   void CreadWaveData()
    {
        GameWaveData gameWaveData = new GameWaveData();
        gameWaveData.Datas = new List<Data>();
        GameDB GameDB = Resources.Load("AssetsBundle/Data", typeof(GameDB)) as GameDB;
        List<WaveData> waveDatas = GameDB.waveDatas;
        for (int i = 0; i < waveDatas.Count; i++) //关卡
        {
          WaveData waveData = waveDatas[i];
          string usesceneindex = waveData.UseScene.Split('-')[0];
          string areaID = waveData.UseScene.Split('-')[1];
          UnityEngine.Object io=  AssetDatabase.LoadAssetAtPath(Constant.projectPath+ Constant.zombiesPosPath+ string.Format(Constant.zombiesPosDataName,usesceneindex),typeof(ZombiesDotData));
          if (io)
          {
              DebugUtility.DebugLogError("缺少场景=》"+usesceneindex+"关卡文件");
              return;
          }
          ZombiesDotData zombiesDotData = io as ZombiesDotData;
          SceneAreaData sceneAreaData = zombiesDotData.sceneDotDatas.Find(f => f.AreaID == areaID);
          string[] bossarray = waveData.BossData.Split('/');
          Data data = new Data();
          data.Level = i;
          data.LevelData = new List<Wavedata>();
          for (int z = 0; z < sceneAreaData.sceneDotDatas.Count;z++) //波次数据
          {
                Wavedata wavedata = new Wavedata();
                wavedata.wavename = "LevelWave-"+ (sceneAreaData.sceneDotDatas[z].waveid+1);
                wavedata.wave = sceneAreaData.sceneDotDatas[z].waveid;
                wavedata.zombiesArray = GetZombiesID(waveData.ZombiesCount, bossarray.Length, sceneAreaData.sceneDotDatas[i].poscount, GameDB,z);
                data.LevelData.Add(wavedata);
          }
            gameWaveData.Datas.Add(data);
        }

        AssetDatabase.CreateAsset(gameWaveData, Constant.projectPath+"/LevelData.Asset");
        Debug.LogError((Constant.projectPath + "/LevelData.Asset"));
        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath((Constant.projectPath + "/LevelData.Asset"), typeof(GameWaveData));
#if UNITY_ANDROID
        BuildTarget target = BuildTarget.Android;
#elif UNITY_IOS
		BuildTarget target = BuildTarget.iOS;
#else
        BuildTarget target = BuildTarget.StandaloneWindows64;
#endif
        BuildPipeline.BuildAssetBundle(o, null, Constant.AssetBundlePath + ("LevelData" + ".unity3d"), BuildAssetBundleOptions.None, target);
        DebugUtils.LogError("Build WaveData Success! "+"Path=>"+ Constant.AssetBundlePath + ("LeveData" + ".unity3d"));
    }


    public static List<int> GetZombiesID(int count ,int boosCount,int wavecount, GameDB GameDB,int z)
    {
        List<ZombiesData> ZombisDatas = GameDB.zombiesDatas.FindAll(f=>f.ZombiesType== DBZombiesType.Batman);
        List<ZombiesData> BossDatas = GameDB.zombiesDatas.FindAll(f => f.ZombiesType == DBZombiesType.Explosion);
        List<int> zomlist = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int id = ZombisDatas[Random.Range(0, ZombisDatas.Count)].ZombiesID;
            zomlist.Add(id);
        }
        if (wavecount >= z)
            zomlist[count - 1] = BossDatas[Random.Range(0, BossDatas.Count)].ZombiesID;
        return zomlist;
    }

    [MenuItem("Tools/CreationSceneData")]
    public static void CreationScenePosData()
    {
        ZombiesDotData zombiesDotData = new ZombiesDotData();
        GameObject obj = Selection.activeGameObject;
        zombiesDotData.sceneDotDatas = new List<SceneAreaData>();
        if (obj != null)
        {
            Transform _map = obj.transform.Find("Zombies");
            int Areacount = _map.childCount;

            for (int i = 0; i < Areacount; i++)
            {
                SceneAreaData sceneAreaData = new SceneAreaData();
                sceneAreaData.sceneDotDatas = new List<SceneDotData>();
                sceneAreaData.AreaID = _map.GetChild(i).name;
                Transform root = _map.GetChild(i);
                int wavecount = root.childCount;
                for (int j = 0; j < wavecount; j++)
                {
                    SceneDotData sceneDotData = new SceneDotData();
                    sceneDotData.waveid = j;
                    sceneDotData.poscount = root.childCount;
                    sceneAreaData.sceneDotDatas.Add(sceneDotData);
                }
                zombiesDotData.sceneDotDatas.Add(sceneAreaData);
            }
            string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            WriteAssetsBundle(zombiesDotData, name.Remove(0,5));
        }
    }
    private static void WriteAssetsBundle(ZombiesDotData db,string sceneindex)
    {
        if(!System.IO.Directory.Exists(Constant.projectPath + Constant.zombiesPosPath))
        {
            System.IO.Directory.CreateDirectory(Constant.projectPath + Constant.zombiesPosPath);
        }
        DebugUtils.LogError(string.Format(Constant.zombiesPosDataName, sceneindex));
        string path = Constant.projectPath + Constant.zombiesPosPath + (string.Format(Constant.zombiesPosDataName, sceneindex));
        //创建“资源文件”，可在inspector查看和编辑
        AssetDatabase.CreateAsset(db, path);
        /*
        //将“资源文件”创建成AssetBundle文件
        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(path, typeof(ZombiesDotData));

#if UNITY_ANDROID
        BuildTarget target = BuildTarget.Android;
#elif UNITY_IOS
		BuildTarget target = BuildTarget.iOS;
#else
        BuildTarget target = BuildTarget.StandaloneWindows64;
#endif
        BuildPipeline.BuildAssetBundle(o, null, Constant.AssetBundlePath+("Scene"+sceneindex+ ".unity3d"), BuildAssetBundleOptions.None, target);

        //如无需在inspector修改“资源文件”的数据，可删除
        //AssetDatabase.DeleteAsset(assetsPath);
        */
        //刷新
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log("Build Assets Success!    (Data Path: " + Constant.AssetBundlePath + ")");
    }
    #endregion
}
