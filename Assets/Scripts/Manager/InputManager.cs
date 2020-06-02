using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using HedgehogTeam.EasyTouch;
public class InputManager : EventBasic
{
    public GameObject _player_Collier;
    public override void Awake()
    {
        base.Awake();
        GameContorlManager._Instance._inputManager = this;
    }
    public  void Start()
    {
        _player_Collier = GameObject.FindGameObjectWithTag(Tag.player);
    }

    public float _minangle;
    public float _maxabgle;
    public void Swipe(Vector2 pos)
    {

        return;
        _player_Collier.transform.localEulerAngles = new Vector3(ClampAngle(_player_Collier.transform.localEulerAngles.x- pos.y),
                                                                 ClampAngle(_player_Collier.transform.localEulerAngles.y+ pos.x),0);
    }
       
    public float ClampAngle(float angle)
    {
        var _angle = NormalizeAngle(angle);
        return _angle;
    }

    /// Normalize an angle between -180 and 180 degrees.
    /// <param name="angleDegrees">angle to normalize</param>
    /// <returns>normalized angle</returns>
    private float NormalizeAngle(float angleDegrees)
    {
        while (angleDegrees > 180f)
        {
            angleDegrees -= 360f;
        }

        while (angleDegrees <= -180f)
        {
            angleDegrees += 360f;
        }
        return angleDegrees;
    }
}
/*Test
public void OnSwipe(Gesture gesture)
{
    _rotationX -= gesture.deltaPosition.y * Time.deltaTime * 5;
    _rotationX = Mathf.Clamp(_rotationX, -30f, 16f);
    _rotationY += gesture.deltaPosition.x * Time.deltaTime * 5;
    _rotationY = Mathf.Clamp(_rotationY, -20, 20);
    Vector3 startpos = _player_Camear.transform.localEulerAngles;
    _player_Camear.transform.rotation =Quaternion.Euler(_player_Camear.transform.localEulerAngles.y+ gesture.deltaPosition.x, _player_Camear.transform.localEulerAngles.x + gesture.deltaPosition.y, _player_Camear.transform.localEulerAngles.z);
    //_player_Camear.transform.localEulerAngles = new Vector3(_player_Camear.transform.localEulerAngles.x+_rotationX, _player_Camear.transform.localEulerAngles.y+_rotationY, 0);
}
  */

/*
public void OnSwipe(Gesture gesture)
{
    _rotationX -= gesture.deltaPosition.y * Time.deltaTime * 5;
    _rotationX = Mathf.Clamp(_rotationX, -30f, 16f);
    _rotationY += gesture.deltaPosition.x * Time.deltaTime * 5;
    _rotationY = Mathf.Clamp(_rotationY, -20, 20);
    Vector3 startpos = _player_Camear.transform.localEulerAngles;
    _player_Camear.transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0);
}
*/
