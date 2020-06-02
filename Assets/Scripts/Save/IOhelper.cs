using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class IOhelper 
{

    public static bool GetIsFileExists(string fileName)
    {
        return File.Exists(fileName);
    }

    public static bool GetIsDirectoryExists(string directoreNamePath)
    {
        return Directory.Exists(directoreNamePath);
    }

    public static void CreateFile(string  fileName,string content)
    {
        StreamWriter streamWriter = File.CreateText(fileName);
        streamWriter.Write(content);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    public static void CreateDiretore(string filename)
    {
        if (!GetIsDirectoryExists(filename))
            Directory.CreateDirectory(filename);
    }

    public static void DeleteFile(string path)
    {
        File.Delete(path);
    }

    public static void SetData(string path,object data)
    {
        string StrData = ObjToString(data);
        StrData = StringEncryption.EncryptDES(StrData,Constant.DussecretKey);       
        if (!GetIsDirectoryExists(path))
         {
            CreateDiretore(path);
         }

        Debug.LogError(path + Constant.DataName+"                 DIR");
        CreateFile(path+Constant.DataName, StrData);
    }

    public static T GteData<T>(string path)
    {
        StreamReader streamReader = File.OpenText(path+ Constant.DataName);
        string data = streamReader.ReadToEnd();
        data= StringEncryption.DecryptDES(data,Constant.DussecretKey);
        return StringToObj<T>(data);
    }


    public static string ObjToString(object data)
    {
        return JsonUtility.ToJson(data);
    }

    public static T StringToObj<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }

}
