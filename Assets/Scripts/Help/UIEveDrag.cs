using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEveDrag :MonoBehaviour,IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private GameObject obj;
    public delegate void MyDelege(PointerEventData obj);
    public MyDelege onDrag;
    public MyDelege onEndDrag;
    public MyDelege onBeginDrag;

    public static UIEveDrag Get(GameObject mobj) 
    {
        UIEveDrag uIEveDrag = mobj.GetComponent<UIEveDrag>();
        if (uIEveDrag==null) 
        {
            uIEveDrag = mobj.AddComponent<UIEveDrag>();
        }
        return uIEveDrag;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null)
            onBeginDrag(eventData);
    }

    public  void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null)
            onEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
            onDrag(eventData);
    }
}
