using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Resource : MonoBehaviour
{
    private static Resource resource;
    SaveManangr SaveManangr;
    private void Awake()
    {
        SaveManangr = SaveManangr.Get;
        resource = this;
#if ANDORID
        Debug.unityLogger.logEnabled = false;
#endif
        //ResourceMananger.AddPalyerObj(playerObjs[i].id, playerObjs[i].GameObject);
    }

    public List<Zomboes> Zomboes;
    public void LoadPlayerObj(string path="Zombies")
    {
        if (!Directory.Exists("Assets/Resources/" + path))
        {
            Debug.LogError("不存在这个文件夹");

        }
        else
        {
            if (Zomboes == null)
                Zomboes = new List<Zomboes>();
            else
                Zomboes.Clear();

            GameObject[] gameObjects = Resources.LoadAll<GameObject>(path);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Zomboes m_playerObjs = new Zomboes((DBZombiesID)System.Enum.Parse(typeof(DBZombiesID), gameObjects[i].name), gameObjects[i]);
                Zomboes.Add(m_playerObjs);
            }
        }
    }


    public static Resource Get { get => resource; }
}

//--
[System.Serializable]
public class Zomboes
{
    public Zomboes(DBZombiesID wave, GameObject gameObject = null)
    {
        this.ID = wave;
        GameObject = gameObject;
    }
    public DBZombiesID ID;
    public GameObject GameObject;
}



