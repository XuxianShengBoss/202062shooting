using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainPanel : MonoBehaviour
{
    public Image _uiDragMove;
    public Image _uiDragBg;
    private float _uiDragBgR;
    private Vector3 _uidragStart_pos;
    private Vector3 _dragStartVec;
    private Vector3 _dragExitVec;
    private void Start()
    {
        Info();
        AddEvent();
    }

    public void Info() 
    {
        RectTransform _uidragBgRect = _uiDragBg.transform as RectTransform;
        _uiDragBgR = _uidragBgRect.sizeDelta.x/2;
        _uidragStart_pos = _uiDragMove.transform.position;
    }
    public void InfoComponent() 
    {
    

    }
    public void AddEvent() 
    {
        UIEveDrag _uiDragMoveUiEvent = UIEveDrag.Get(_uiDragMove.gameObject);
        /*
        _uiDragMoveUiEvent.onDrag += OnUIDragMove;
        _uiDragMoveUiEvent.onBeginDrag += OnStartOnDrag;
        _uiDragMoveUiEvent.onEndDrag += OnExitDrag;
        */
    }

    public void OnStartOnDrag(Vector3 vector)
    {
        _dragStartVec = vector;
    }
    public void OnUIDragMove(Vector3 vector) 
    {        
        _dragExitVec = vector;     
        float dir=(_dragExitVec - _dragStartVec).magnitude;
        if (dir <= _uiDragBgR)
        {
           _uiDragMove.transform.position = _uidragStart_pos + (_dragExitVec - _dragStartVec);
        }
        else 
        {
          Vector3 exitpos = _dragExitVec- _dragStartVec;
          _uiDragMove.transform.position = _uidragStart_pos+ exitpos.normalized * _uiDragBgR;
        }
        
    }
    public void OnExitDrag(Vector3 vector) 
    {
        _uiDragMove.transform.position = _uidragStart_pos;
    }

}
