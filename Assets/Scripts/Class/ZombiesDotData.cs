using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombiesDotData : UnityEngine.ScriptableObject
{
    public List<SceneAreaData> sceneDotDatas;
}

[System.Serializable]
public struct SceneAreaData
{
    public string AreaID;
    public List<SceneDotData> sceneDotDatas;
}

[System.Serializable]
public struct SceneDotData
{
    public int waveid;
    public int poscount;
}
