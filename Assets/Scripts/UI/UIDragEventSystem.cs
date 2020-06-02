using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragEventSystem : MonoBehaviour,IDragHandler
{
    private  UIDragEventSystem system;
    public delegate void Dele(PointerEventData data);
    public Dele _OnDrag; 
    public  UIDragEventSystem Get(GameObject @object)
    {
        if (system = null)
            system = @object.AddComponent<UIDragEventSystem>();
        return system;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _OnDrag?.Invoke(eventData);
    }
}
