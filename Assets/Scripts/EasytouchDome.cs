using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
using HedgehogTeam.EasyTouch;
public class EasytouchDome : MonoBehaviour
{
    public Transform _player_Camear;
    private void Start()
    {
        EasyTouch.On_Swipe += OnSwipe;
        EasyTouch.On_Swipe2Fingers+=delegate {

            Debug.LogError("dsadsaddsassd");
     	};
        _rotationX = _player_Camear.transform.eulerAngles.x;
    }
    void LateUpdate()
    {
        return;
        Gesture gesture = EasyTouch.current;
        if (gesture != null)
        {
            if (gesture.type == EasyTouch.EvtType.On_Swipe&&gesture.touchCount==1)
            {
                //OnSwipe(gesture);
            }
        }
    }

   public  float _rotationX;
   public float _rotationY;
    public void OnSwipe(Gesture gesture)
    {
        _rotationX -= gesture.deltaPosition.y* Time.deltaTime*5;
        _rotationX = Mathf.Clamp(_rotationX, -30f, 16f);
        _rotationY += gesture.deltaPosition.x * Time.deltaTime*5;
        _rotationY = Mathf.Clamp(_rotationY,-20,20);
        Vector3 startpos = _player_Camear.transform.localEulerAngles;
        _player_Camear.transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0);
    }


    private void EasyTouch_On_Twist(Gesture gesture)
    {
        _player_Camear.Rotate(Vector3.up * gesture.twistAngle);
        EasyTouch.SetEnablePinch(false);
      
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
*/