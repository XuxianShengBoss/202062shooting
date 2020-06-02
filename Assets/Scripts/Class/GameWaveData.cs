using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelData
{
    public class GameWaveData : UnityEngine.ScriptableObject
    {
       public List<Data> Datas;
    }

    [System.Serializable]
    public struct Data
    {
        public int Level;
        public List<Wavedata> LevelData;//mei一波次僵尸数据
    }

    [System.Serializable]
    public struct Wavedata
    {
        [HideInInspector]
        public string wavename;
        public int wave;
        public List<int> zombiesArray;
    }
        
}