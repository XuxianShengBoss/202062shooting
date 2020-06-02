using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void MyDele<T, V, K, Z>(T arg1, V arg2, K arg3, Z arg4);
    public delegate void MyDele<T, V, K>(T arg1, V arg2, K arg3);
    public delegate void MyDele<T, V>(T arg1, V arg2);
    public delegate void MyDele<T>(T arg1);
    public delegate void MyDele();
    static Dictionary<string, Action<object>> mRegisteredMsgs = new Dictionary<string, Action<object>>();
    static Dictionary<UIButtonEvent, Delegate> MsgsDic = new Dictionary<UIButtonEvent, Delegate>();
    public static void Register(string msgName, Action<object> onMsgReceived)
    {
        if (!mRegisteredMsgs.ContainsKey(msgName))
        {
            mRegisteredMsgs.Add(msgName, _ => { });
        }

        foreach (var item in mRegisteredMsgs[msgName].GetInvocationList())
        {
            if(item.GetType() == onMsgReceived.GetType())
            {
                mRegisteredMsgs[msgName] -= onMsgReceived;
                break;
            }
        }
        mRegisteredMsgs[msgName] += onMsgReceived;
    }
    #region
    //--
    public static void AddEvent<T>(UIButtonEvent uIButtonEvent, MyDele<T> myDele)
    {
        if (!MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Add(uIButtonEvent, null);
        }
        MsgsDic[uIButtonEvent] = (MyDele<T>)MsgsDic[uIButtonEvent] + myDele;
    }

    public static void AddEvent(UIButtonEvent uIButtonEvent, MyDele myDele)
    {
        if (!MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Add(uIButtonEvent, null);
        }
        MsgsDic[uIButtonEvent] = (MyDele)MsgsDic[uIButtonEvent] + myDele;
    }

    public static void AddEvent<T, V>(UIButtonEvent uIButtonEvent, MyDele<T, V> myDele)
    {
        if (!MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Add(uIButtonEvent, null);
        }
        MsgsDic[uIButtonEvent] = (MyDele<T, V>)MsgsDic[uIButtonEvent] + myDele;
    }

    public static void AddEvent<T, V, K>(UIButtonEvent uIButtonEvent, MyDele<T, V, K> myDele)
    {
        if (!MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Add(uIButtonEvent, null);
        }
        MsgsDic[uIButtonEvent] = (MyDele<T, V, K>)MsgsDic[uIButtonEvent] + myDele;
    }
    public static void AddEvent<T, V, K,Z>(UIButtonEvent uIButtonEvent, MyDele<T, V, K,Z> myDele)
    {
        if (!MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Add(uIButtonEvent, null);
        }
        MsgsDic[uIButtonEvent] = (MyDele<T, V, K,Z>)MsgsDic[uIButtonEvent] + myDele;
    }

    public static void Broadcast(UIButtonEvent uIButtonEvent)
    {
        Delegate d = null;
        if (MsgsDic.ContainsKey(uIButtonEvent))
            MsgsDic.TryGetValue(uIButtonEvent, out d);
        if (d != null)
        {
            MyDele my = d as MyDele;
            my();
        }
    }

    public static void Broadcast<T>(UIButtonEvent uIButtonEvent, T arg)
    {
        Delegate d = null;
        if (MsgsDic.ContainsKey(uIButtonEvent))
            MsgsDic.TryGetValue(uIButtonEvent, out d);
        if (d != null)
        {
            MyDele<T> my = d as MyDele<T>;
            my(arg);
        }
    }

    public static void Broadcast<T, V>(UIButtonEvent uIButtonEvent, T arg1, V arg2)
    {
        Delegate d = null;
        if (MsgsDic.ContainsKey(uIButtonEvent))
            MsgsDic.TryGetValue(uIButtonEvent, out d);
        if (d != null)
        {
            MyDele<T, V> my = d as MyDele<T, V>;
            my(arg1, arg2);
        }
    }

    public static void Broadcast<T, V, K>(UIButtonEvent uIButtonEvent, T arg1, V arg2, K arg3)
    {
        Delegate d = null;
        if (MsgsDic.ContainsKey(uIButtonEvent))
            MsgsDic.TryGetValue(uIButtonEvent, out d);
        if (d != null)
        {
            MyDele<T, V, K> my = d as MyDele<T, V, K>;
            my(arg1, arg2, arg3);
        }
    }

    public static void Removemag(UIButtonEvent uIButtonEvent)
    {
        Delegate d = null;
        if (MsgsDic.ContainsKey(uIButtonEvent))
        {
            MsgsDic.Remove(uIButtonEvent);            
        }
    }

    #endregion

    public static void UnRegisterAll(string msgName)
    {
        mRegisteredMsgs.Remove(msgName);
    }

    public static void UnRegister(string msgName, Action<object> onMsgReceived)
    {
        if (mRegisteredMsgs.ContainsKey(msgName))
        {
            mRegisteredMsgs[msgName] -= onMsgReceived;
        }
    }

    public static void Send(string msgName, object data)
    {
        if (mRegisteredMsgs.ContainsKey(msgName))
        {
            mRegisteredMsgs[msgName](data);
        }
    }
}

public  enum UIButtonEvent
{    
  One,
  SetBulletCountText,
}
