
using UnityEngine;
using gungame;
using System.Collections.Generic;
using LevelData;

public class MyGame : MonoBehaviour
{
    public bool IsEmptyData;
    public static GameDB GameDB;
    public static List<WaveData> WaveData;
    public static List<ZombiesData> zombiesDatas;
    public static GameWaveData _GameWaveData;

    private void Awake()
    {
        Create();
        DontDestroyOnLoad(gameObject);
        if (IsEmptyData)
            EmptyData();
        SaveManangr.Get.InstanceObj();
    }




    private void Create()
    {
        GameDB = Resources.Load("AssetsBundle/Data", typeof(GameDB)) as GameDB;
        _GameWaveData = Resources.Load("AssetsBundle/LevelData", typeof(GameWaveData)) as GameWaveData;
        WaveData = new List<WaveData>();
        zombiesDatas = new List<ZombiesData>();
        WaveData = GameDB.waveDatas;
        zombiesDatas = GameDB.zombiesDatas;
        SaveManangr.Get.InstanceObj();
    }

    public void EmptyData()
    {
#if UNITY_EDITOR

        if (UnityEditor.EditorUtility.DisplayDialog("", "是否清理数据", "确定", "取消"))
        {
            IOhelper.DeleteFile(Constant.SavePaht + Constant.DataName);
            Debug.LogError("数据清理完成~~~~~~~~~~~~~~~~~~~~~~~");
        }
#endif
    }

    void OnDestroy()
    {
       
    }

    private void OnApplicationPause(bool isPause)
    {

    }

    private void OnApplicationFocus(bool isFocus)
    {

    }
}