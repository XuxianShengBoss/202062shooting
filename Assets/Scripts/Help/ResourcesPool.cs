using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesPool : MonoBehaviour
{
    private static ResourcesPool _instance;
    public GameObject bulletprefab;
    public List<GameObject> bulletList;    
    public static ResourcesPool Get
    {
        get
        {
            if (_instance==null)
            {
                _instance = new GameObject("ResourcesPool").AddComponent<ResourcesPool>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        bulletList = new List<GameObject>();        
    }

    public void PreLoadPredab()
    {
        for (int i = 0; i < 30; i++)
        {
           GameObject bullet= GameObject.Instantiate<GameObject>(bulletprefab,this.transform,true);
           bullet.SetActive(false);
           bulletList.Add(bullet);
        }
    }

    public  GameObject GetBullet()
    {
        GameObject bulletobj = null;
        if (bulletList.Count <= 0)
        {
            PreLoadPredab();
        }
        if (bulletList.Count > 0)
        {
            bulletobj = bulletList[0];
        }
        if (bulletobj != null)
        {
            bulletList.Remove(bulletobj);
            return bulletobj;
        }
        return null;
    }

    public void AddBulletObj(GameObject obj)
    {
        if (bulletList != null)
        {           
            bulletList.Add(obj);
        }
    }

    public void SetBulletParent()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i].transform.parent != this.transform)
                bulletList[i].transform.parent = this.transform;
        }
    }
}
