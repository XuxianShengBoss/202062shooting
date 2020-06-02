using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMananger : MonoBehaviour
{
    public static ResourceMananger _resourceMananger;
    private void Start()
    {
        _resourceMananger = this;
        DontDestroyOnLoad(this.gameObject);
    }

    #region EnemyPrefab
    public static Dictionary<int, GameObject> WaveDic = new Dictionary<int, GameObject>();
    public static Dictionary<int, GameObject> PalyerDic = new Dictionary<int, GameObject>();
    public static GameObject GetEnemyObj(int dBEnemy)
    {
        if(WaveDic.ContainsKey(dBEnemy))
            return WaveDic[dBEnemy].gameObject;
        Debug.LogError("暂未有此id的对象=》》》》" + dBEnemy);
        return null;
    }
    public static void AddEnemyObj(int dBEnemy, GameObject gameObject)
    {
        if (!WaveDic.ContainsKey(dBEnemy))
            WaveDic.Add(dBEnemy, gameObject);
    }
    #endregion

    #region EnemyPrefab
    public static GameObject GetPalyerObj(int dBEnemy)
    {
        if (PalyerDic.ContainsKey(dBEnemy))
            return PalyerDic[dBEnemy].gameObject;
        Debug.LogError("暂未有此id的对象=》》》》" + dBEnemy);
        return null;
    }
    public static void AddPalyerObj(int dBEnemy, GameObject gameObject)
    {
        if (!WaveDic.ContainsKey(dBEnemy))
            WaveDic.Add(dBEnemy, gameObject);
    }
    #endregion
}

