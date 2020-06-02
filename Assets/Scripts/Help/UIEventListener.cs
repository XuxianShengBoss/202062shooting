using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;

public class UIEventListener :MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public static UIEventListener Get(GameObject obj)
    {
        UIEventListener uIEventListener = obj.GetComponent<UIEventListener>();
        if (uIEventListener == null)
        {
            uIEventListener = obj.AddComponent<UIEventListener>();
            Image image = obj.GetComponent<Image>();
            if (image != null)
                image.raycastTarget = true;
            Text text = obj.GetComponent<Text>();
            if (text != null)
                text.raycastTarget = true;
        }
        return uIEventListener;
    }
   // 定义事件代理
    public delegate void UIEventProxy(GameObject gb);

    // 鼠标点击事件
    public event UIEventProxy OnClick;

    // 鼠标进入事件
    public event UIEventProxy OnMouseEnter;

    // 鼠标滑出事件
    public event UIEventProxy OnMouseExit;

    // 鼠标按下事件
    public event UIEventProxy OnMouseDown;

    // 鼠标抬起事件
    public event UIEventProxy OnMouseUp;

    public delegate void EventTriggerEvent(GameObject obj,float value);
    public EventTriggerEvent ArgEvent;

    /*
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (ArgEvent != null)
        {
            ArgEvent(this.gameObject,gameObject.GetComponent<Slider>().value);
        }
        //base.OnEndDrag(eventData);
    }
    */

    public void Clear() 
    {
        if (OnClick != null) 
        {
            MethodInfo methodInfo = OnClick.Method;
            if (methodInfo != null)
                methodInfo = null;
            else 
            {
                Delegate[] delelist= OnClick.GetInvocationList();
                delelist = null;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
        {                    
            OnClick(this.gameObject);            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnMouseEnter != null)
            OnMouseEnter(this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnMouseDown != null)
            OnMouseDown(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnMouseExit != null)
            OnMouseExit(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnMouseUp != null)
            OnMouseUp(this.gameObject);
    }    
}