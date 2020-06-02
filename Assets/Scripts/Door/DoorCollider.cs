using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
     Animation _ani;
    void Start()
    {
        _ani=this.transform.GetComponentInChildren<Animation>();
    }
void OnTriggerEnter(Collider other)
{
     if(other.transform.tag==Tag.player)
     {
         _ani["ExteriorDoor"].time=0;
         _ani.Play();
     }
}

void OnTriggerExit(Collider other)
{
    if(other.transform.tag!=Tag.player) return;
    _ani["ExteriorDoor"].time=_ani["ExteriorDoor"].clip.length;
    _ani["ExteriorDoor"].speed=-1;
    _ani.Play();
}
}
