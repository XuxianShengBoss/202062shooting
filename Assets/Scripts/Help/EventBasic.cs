using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBasic : MonoBehaviour
{
    public virtual void Awake()
    {
      AddEvent();   
    }

    public virtual void Inif() { }
    public virtual void AddEvent() { }
    public virtual void RemoveEvent() { }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}
