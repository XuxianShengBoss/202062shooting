using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public float _startangle = 0;
    public float _minangle=30;
    public float _maxabgle=30;
    public Animator _ani;
    private void Start()
    {

        //_startangle = NormalizeAngle(this.transform.localEulerAngles.y);
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawLine(ray.origin, ray.direction * 1000, Color.red);
    }




    /*
    // Update is called once per frame
    void Update()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        Debug.LogError(this.transform.localEulerAngles.y + "his.transform.localEulerAngles.y");
        float time= _ani.GetCurrentAnimatorStateInfo(0).length;
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x,ClampAngle(this.transform.localEulerAngles.y - _horizontal), 0);      
    }

    public float ClampAngle(float angle)
    {
        var _angle = NormalizeAngle(angle);
        _angle = Mathf.Clamp(_angle, _startangle - _minangle, _startangle + _maxabgle);
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
    */

    public Transform rotate;
    private void OnAnimatorIK(int layerIndex)
    {
        Debug.LogError(layerIndex);
        if (layerIndex == 0)
        {
            _ani.SetLookAtPosition(rotate.position);
            _ani.SetLookAtWeight(1);
        }
    }
}
