using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManger : MonoBehaviour
{
    public Transform _zomboesPos_root;
    public Dictionary<int, List<Transform>> zombiesActiveDic = new Dictionary<int, List<Transform>>();
    private int _waveIndex;
    private  void Awake()
    {
        GameContorlManager._Instance._waveManger = this;
        OnInfoData();
        _waveIndex = 0;
    }

    public void OnInfoData()
    {
        for (int i = 0; i < _zomboesPos_root.childCount; i++)
        {
            zombiesActiveDic.Add(i,new List<Transform>());
            foreach (Transform pos in _zomboesPos_root.GetChild(i).transform)
            {
                zombiesActiveDic[i].Add(pos);
            }
        }
    }

    public List<Transform> GetZombiesPosArray()
    {
        if (_waveIndex < zombiesActiveDic.Count)
        {
        List<Transform> transforms = zombiesActiveDic[_waveIndex];
            _waveIndex++;
            return transforms;
        }
        return null;
    }
}

