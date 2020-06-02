using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using HedgehogTeam.EasyTouch;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiContorlManager : EventBasic
{
    public GameObject _onShooting_button;
    public GameObject _onHuanDan_button;
    public GameObject _Sniper_button;

    public GameObject _OnDrag_image;
    public UnityAction _OnShot;
    public UnityAction _OnHunDan;
    public UnityAction<bool> _OnSniper;
    
  
    public float _OnDrag_spped=1;

    public Text _bulletcount_text;
    public override void Awake()
    {
        base.Awake();
        GameContorlManager._Instance._uiContorlManager = this;
    }

    public override void AddEvent()
    {
        UIEventListener.Get(_onShooting_button).OnClick += delegate {
            _OnShot?.Invoke();
        };
        UIEventListener.Get(_onHuanDan_button).OnClick += delegate
        {
            _OnHunDan?.Invoke();
        };
        UIEventListener.Get(_Sniper_button).OnClick += delegate {
            _OnSniper?.Invoke(true);
        };
       EventManager.AddEvent<int>( UIButtonEvent.SetBulletCountText,SetBulletText);
    }


    public void SetBulletText(int count)
    {
      _bulletcount_text.text=count.ToString();
    }

    public override void RemoveEvent()
    {
EventManager.Removemag( UIButtonEvent.SetBulletCountText);
    }
}
