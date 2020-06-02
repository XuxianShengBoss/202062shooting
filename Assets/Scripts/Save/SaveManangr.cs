using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Class;
using System;


public class SaveManangr : MonoBehaviour
{
    public PlayerInfo m_playerInfo;
    private static SaveManangr m_saveManangr;

    public static SaveManangr Get
    {
        get
        {
            if (m_saveManangr == null)
            {
                m_saveManangr = new GameObject("SaveMananger").AddComponent<SaveManangr>();
            }
            return m_saveManangr;
        }
    }

    private void Awake()
    {
        IOhelper.CreateDiretore(Constant.SavePaht);
        if (!IOhelper.GetIsFileExists(Constant.SavePaht + Constant.DataName))
        {
            DataInitialize();
        }
        GetPlayinfo();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 手动存储数据
    /// </summary>
    public void ManualSave()
    {
        SetPlayerInfo();
    }

    void DataRe(PlayerInfo playerInfo)
    {
        m_playerInfo = playerInfo;
        //if(m_playerInfo==null)
        //--数据添加 新版本
    }

    #region PlayerInfo get set

    private void DataInitialize()
    {
        m_playerInfo = new PlayerInfo();
        m_playerInfo.wave = 1;
        m_playerInfo.mainWeapon = 2;
        m_playerInfo.subWeapon = 1;
        m_playerInfo.BGMMute = false;
        m_playerInfo.AudioMute = false;
        m_playerInfo.limit = 0.5f;//灵敏度
        m_playerInfo.money = 0;
        m_playerInfo.playerWeaponList = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            m_playerInfo.playerWeaponList.Add(i == 0 ? 1 : i == 1 ? 1 : 0);
        }
        m_playerInfo.Level = 1;
        m_playerInfo.Exp = 0;
        m_playerInfo.FirstEnterGame = true;
        m_playerInfo.ReviveCoins = 3;
        SetPlayerInfo();
    }
    //同步数据
    private void GetPlayinfo()
    {
        PlayerInfo playerInfo = IOhelper.GteData<PlayerInfo>(Constant.SavePaht);
        DataRe(playerInfo);
    }

    private void SetPlayerInfo()
    {
        IOhelper.SetData(Constant.SavePaht, m_playerInfo);
        // Help.BackUpData(m_playerInfo);
    }


    void OnApplicationQuit()
    {
        SetPlayerInfo();
    }

    private void OnApplicationPause(bool pause)
    {
        SetPlayerInfo();
    }
    public void InstanceObj()
    {
        Debug.Log("~~~~~~~~~~~~");
    }

    public PlayerInfo PlayerInfo { get => m_playerInfo; set => m_playerInfo = value; }
    #endregion

}
