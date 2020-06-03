using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    /*
    public float _startangle = 0;
    public float _minangle=30;
    public float _maxabgle=30;
    public Animator _ani;
    */
    public Transform _target;
    public Transform red;
    public Transform _move;
    public float x, y,z;
    public float r = 10;
    public Vector3 Vector3 = Vector3.forward;
    private void Start()
    {
        x = red.transform.position.x;
        y = red.transform.position.y;
        z = red.transform.position.z;
        r = Vector3.Distance(_target.transform.position, red.transform.position);
    }

    private void Update()
    {
        float m_x = Input.GetAxis("Horizontal");
        float m_y= Input.GetAxis("Vertical");
        if (m_x != 0 || m_y != 0) 
        {
            x += m_x;
            y += m_y;
        }
        Debug.DrawLine(_target.transform.position, red.transform.position, Color.blue);
        red.transform.position= new Vector3(x, y, z);
        Vector3 nor = _target.position - new Vector3(x,y,z);
        _move.transform.position = _target.position - nor.normalized * r;
        Debug.DrawLine(_target.transform.position, _target.position - nor.normalized * r,Color.red);
        
       
       
        // Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5fInput.GetAxis("Vertical");, 0.5f, 0));
        // Debug.DrawLine(ray.origin, ray.direction * 1000, Color.red);
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

    /*
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
    */
}
