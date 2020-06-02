using System.Collections;
using System.Collections.Generic;
using LevelData;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int _Level;
    public GameWaveData GameWaveData;

    public override void Awake()
    {
        Info();
    }

    public void Info()
    {
        GameWaveData = Resources.Load(Constant.AssetBundlePath + "LeveData.unity3d") as GameWaveData;
    }
}
