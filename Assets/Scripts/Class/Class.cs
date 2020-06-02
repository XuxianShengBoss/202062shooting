using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  Class 
{
    [Serializable]
    public class ListLevelData
    {
        public int Level;
        public List<waveData> waveDatas;
    }

    [Serializable]
    public class waveData
    {        
        public int wave;
        public List<int> zombiesIdList;       
    }
}

[System.Serializable]
public class PlayerInfo
{ 
    public int wave;
    public int age;
    public int mainWeapon;
    public int subWeapon;
    public bool BGMMute;
    public bool AudioMute;
    public float limit;
    public int money;
    public List<int> playerWeaponList;
    public int Level;
    public int Exp;
    public List<int> MissionSNumList;
    public List<int> SkillLevelList;
    public List<int> SkillPool;
    public List<int> WeaponLevelList;
    public bool FirstEnterGame;
    public string LastGainTime;
    public string LastDailyGainTime;
    public int ReviveCoins;
}


