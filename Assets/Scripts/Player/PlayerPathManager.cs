using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathManager : MonoBehaviour
{
    MoveBasic[] moveBasics;
    MoveBasic _CurrMoveBasic;
    int index;
    private void Start()
    {
        index = 0;
        moveBasics = this.transform.GetComponentsInChildren<MoveBasic>();
        System.Array.Sort(moveBasics,(a,b)=>a.name.CompareTo(b.name));
        _CurrMoveBasic = moveBasics[index];
    }
    
    private void Update()
    {
        if (_CurrMoveBasic != null && _CurrMoveBasic.m_state == MoveBasic.State.Update)
            _CurrMoveBasic.m_Update();
    }
}
