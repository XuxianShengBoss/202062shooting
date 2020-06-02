using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Image _drag_image;
    public Transform _playerTarget;
    private Vector3 _startPos;
    private Vector3 _offset;

    void Start()
    {
        _startPos = this.transform.position;
        _CemeraY = this.transform.position.y;
        UIEveDrag.Get(_drag_image.gameObject).onDrag += CameraLook;
    }

    /*
    void Update()
    {
         _Roy += Input.GetAxis("Mouse X") * speed;
        Quaternion quaternion = Quaternion.Euler(0, _Roy, 0);
        this.transform.position = _target.position - (quaternion * dirrVer);
        this.transform.LookAt(_target.position);     

        float y = Input.GetAxis("Mouse X");
        float roy = this.transform.localEulerAngles.y;
        roy += y;
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x,roy,this.transform.localEulerAngles.z);
        this.transform.LookAt(_target.position);

   }
    */
    /*
    [SerializeField]
    private Transform Target;
    public float _rotY;
    public float rotSpeed = 1.5f;
    private Vector3 _offset;
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        //存储位移偏移量
        _offset = Target.position - transform.position;
    }
    void Update()
    {

        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0)
        {
            _rotY += horInput * rotSpeed;
        }
        else      
        _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        Quaternion rotation = Quaternion.Euler(0, -_rotY, 0);
        // Quaternion.Euler(0, Y, 0);   基于Y轴的旋转 水平旋转
        // Quaternion.Euler(X, 0, 0);   基于X轴的旋转 垂直旋转
        //一个向量乘以一个四元数（使用Quaternion.Euler方法经选装角度转化为一个四元数，结果是基于旋转偏移位置，得到的是向量基于旋转的偏移位置
        transform.position = Target.position - (rotation * _offset);
        transform.LookAt(Target.position);
    }
       */
    #region
    /*
    return;
    Vector3 rotate = this.transform.eulerAngles;
    float x = this.transform.localEulerAngles.x;
    float _Drad_speed = 0;
    float _Drad_speedZ = 0;
    if (data.delta.y > 0.1f)
    {
        _Drad_speed = 0.1f;
        _Drad_speedZ = -0.1f;
        x -= 0.5f;
    }
    else if (data.delta.y < 0.1f)
    {
        _Drad_speed = -0.1f;
        _Drad_speedZ = 0.1f;
        x += 0.5f;
    }
    if (_Drad_speed != 0)
    {
        _startPos.y = Mathf.Clamp(_startPos.y + _Drad_speed, 1.31f, 3.605f);
        _startPos.z = Mathf.Clamp(_startPos.z + _Drad_speedZ, -1.85f, -2.798f);
        this.transform.position = _startPos;
        x = Mathf.Clamp(x, -14.347f, 32.161f);
        //this.transform.LookAt(_target.position);
        this.transform.rotation = Quaternion.Euler(new Vector3(x, 0, 0));
        //this.transform.localEulerAngles = rotate;
    }
    */
    #endregion

    public float _MinY = 1.31f;
    public float _MaxY = 3.605f;
    public float _CemeraY;
    public void CameraLook(PointerEventData data)
    { 
        Swipe(new Vector2(0, data.delta.y));
        float ro = 0;
        float dargY = 0;
        float dargZ = 0;
        float delatY =Mathf.Abs(data.delta.y) * _MoveSpeed;
        if (data.delta.y > 0.1f)
        {
            ro = delatY / 14f;
            dargY -= ro * (_CemeraY - _MinY);
            dargZ += ro * (_CameraZ-_MinZ);
        }
        else if (data.delta.y<0.1f)
        {
            /*
            ro = delatY / 32f;
            dargY = ro * (_MaxY - _CemeraY);
            dargZ -= ro * (_MaxZ-_CameraZ)*/
        }
        _startPos.z = Mathf.Clamp(_startPos.z+dargZ,-_MaxZ, -_MinZ);
        _startPos.y = Mathf.Clamp(_startPos.y+dargY,_MinY,_MaxY);
        _startPos.x = 0;
        
        if (data.delta.x > 0.1f || data.delta.x < 0.1f)
        {
            /*
            
            _playerTarget.transform.localEulerAngles = new Vector3(ClampAngle(_playerTarget.transform.localEulerAngles.x),
                                                                   Mathf.Clamp(ClampAngle(_playerTarget.transform.localEulerAngles.y + data.delta.x * _MoveSpeed), _PlayerMinAngle, _PlayerMaxAngle)
                                                               ,0);
              */                                                 
        }
        this.transform.localPosition = _startPos;
    }


    public float _minangle=-14f;
    public float _maxabgle=32f;
    public float _MinZ = 1.85f;
    public float _MaxZ = 2.798f;
    public float _CameraZ = 1.96f;
    public float _MoveSpeed=0.2f;
    public float _PlayerMinAngle=-30;
    public float _PlayerMaxAngle=30;
    public void Swipe(Vector2 pos)
    {
        this.transform.localEulerAngles =
            new Vector3(Mathf.Clamp(ClampAngle(this.transform.localEulerAngles.x - pos.y * _MoveSpeed), _minangle, _maxabgle),
                             0, 0);
    }
    //Mathf.Clamp(ClampAngle(this.transform.localEulerAngles.y + pos.x),)
    public float ClampAngle(float angle)
    {
        var _angle = NormalizeAngle(angle);  
        return _angle;
    }

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

