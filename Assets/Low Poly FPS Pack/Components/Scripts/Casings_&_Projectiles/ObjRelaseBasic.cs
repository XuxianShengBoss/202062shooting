using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using ClassEnum;
public class ObjRelaseBasic : MonoBehaviour
{
  public PoolPrefabType type;
  protected virtual void Relase()
  {
   PoolManager.Get.Release(type,this.gameObject);
  }

}
