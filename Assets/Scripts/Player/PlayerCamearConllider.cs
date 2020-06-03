using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RootMotion;
//using UnityEngine.Experimental.PlayerLoop;
using DG.Tweening;

public class PlayerCamearConllider : MonoBehaviour
{
    public static PlayerCamearConllider _instance;
    [SerializeField]
    [Tooltip("围绕主角的小球")]
    public Transform Target;
    [Tooltip("Player父级节点")]
    public Transform _platerRoot;
    public float _rotY;
    public float _rotX;
    public float _CamearTarget_rotX;
    public float _CamearTarget_rotY;
    public float rotSpeed = 1.5f;
    public float inputspeed=0.05f;
    private Vector3 _offset;
    private Vector3 _roundOffset;
    public Vector2 _cemearHor;//-45 35
    public Vector2 _cemearVer;//-45 35    
    public Vector2 _HorRotateConstrainAangle;//-45 35
    private Vector3 _PlayerCapsule;
    private Vector3 _GunCamrar;
    //public Vector2 _VerRotateConstrainAangle;//-15 31
    public CameraControllerFPS _cam;
    //aimZ进行约束大小
    //public Vector2 _Aim_HorZ;

    [Tooltip("玩家")]
    public Transform playertarget;
    //public AimIK AimIK;
    public Image _Drag_image;
    private float _horInput;
    private float _verInput;
    //Aim X =1.81f    -3.38
    //Aim Z =         -0.36
    void Start()
    {
        _instance = this;
        _rotY = NormalizeAngle(transform.localEulerAngles.y);
        _rotX = NormalizeAngle(this.transform.localEulerAngles.x);
        _CamearTarget_rotX =NormalizeAngle(Target.transform.localEulerAngles.x);
        _CamearTarget_rotY =NormalizeAngle(Target.transform.localEulerAngles.y);
        _PlayerCapsule = Target.transform.position;
        _GunCamrar = this.transform.position;
        //存储位移偏移量
        _offset = Target.position - transform.position;
        _roundOffset = playertarget.position - Target.position;
        transform.LookAt(Target.position);
        UIEveDrag.Get(_Drag_image.gameObject).onDrag += Swip;
    }
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        _cam.SetHorVer(new Vector2(horInput, verInput));
        Swip2(horInput, verInput);
        _horInput = 0;
        _verInput = 0;
    }

    public void Swip(PointerEventData eventData) 
    {
        SetHorVer(eventData.delta.x*0.05f, eventData.delta.y*0.05f);
    }

    public void SetHorVer(float horInput, float verInput)
    { 
        _horInput = horInput;
        _verInput = verInput;
    }

    public void Swip2(float horInput, float verInput)
    {
        float angleX = NormalizeAngle(this.transform.localEulerAngles.x);
        float angleY = NormalizeAngle(this.transform.localEulerAngles.y);
        if (horInput != 0 || verInput != 0)
        {
            float _horInput = horInput > 0 ? -horInput : Mathf.Abs(horInput);
            _rotY = (angleY >= _cemearHor.y && _horInput < 0) || (angleY <= _cemearHor.x && _horInput > 0) ? _rotY : _rotY + _horInput ;
            _CamearTarget_rotY = (angleY >= _cemearHor.y && _horInput < 0) || (angleY <= _cemearHor.x && _horInput > 0) ? _CamearTarget_rotY : _CamearTarget_rotY + _horInput ;

            _rotX = (angleX > _cemearVer.y && verInput < 0) || (angleX < _cemearVer.x && verInput > 0) ? _rotX : _rotX - verInput;
            _CamearTarget_rotX = (angleX > _cemearVer.y && verInput < 0) || (angleX < _cemearVer.x && verInput > 0) ? _CamearTarget_rotX : _CamearTarget_rotX - verInput;
        }
        if (horInput != 0 || verInput != 0)
        {
            Quaternion rotation = Quaternion.Euler(_rotX, -_rotY, 0);
            Quaternion camearTarget = Quaternion.Euler(_CamearTarget_rotX, -_CamearTarget_rotY, 0);
            // Quaternion.Euler(0, Y, 0);   基于Y轴的旋转 水平旋转
            // Quaternion.Euler(X, 0, 0);   基于X轴的旋转 垂直旋转
            //一个向量乘以一个四元数（使用Quaternion.Euler方法经选装角度转化为一个四元数，结果是基于旋转偏移位置，得到的是向量基于旋转的偏移位置           
            Target.position = playertarget.position - (camearTarget * _roundOffset);
            Target.LookAt(playertarget.position);
            transform.position = Target.position - (rotation * _offset);
            transform.LookAt(Target.position);
        }       
    }

    public void Swip(float horInput,float verInput)
    {
        if (horInput != 0 || verInput != 0)
            SetHorVer(horInput, verInput);
        float angleX = NormalizeAngle(this.transform.localEulerAngles.x);
        float angleY = NormalizeAngle(this.transform.localEulerAngles.y);
        Vector2 Roatet = Vector2.zero;
        if (_horInput != 0)
        {
            float _horInput =this._horInput > 0 ? -this._horInput : Mathf.Abs(this._horInput);
            _rotY = (angleY >= _cemearHor.y && _horInput < 0) || (angleY <= _cemearHor.x && _horInput > 0) ? _rotY : _rotY + _horInput * rotSpeed;
            _CamearTarget_rotY= (angleY >= _cemearHor.y && _horInput < 0) || (angleY <= _cemearHor.x && _horInput > 0) ? _CamearTarget_rotY : _CamearTarget_rotY + _horInput * rotSpeed;  
            #region
            /*
            float inputhorY = Mathf.Abs(_horInput);            
            Roatet.x = angleY > _HorRotateConstrainAangle.x && angleY < _HorRotateConstrainAangle.y? _horInput : 0;        
           if (horInput > 0)
           {
               float axisangle = AimIK.solver.axis.x;
               axisangle = Mathf.Clamp(axisangle + inputhorY * inputspeed, -_Aim_HorZ.x, _Aim_HorZ.y);
               Vector3 vector3 = AimIK.solver.axis;
               vector3.x = axisangle;
               AimIK.solver.axis = vector3;
           }
           else
           {
               float axisangle = AimIK.solver.axis.x;
               axisangle = Mathf.Clamp(axisangle - inputhorY * inputspeed, -_Aim_HorZ.x, _Aim_HorZ.y);
               Vector3 vector3 = AimIK.solver.axis;
               vector3.x = axisangle;
               AimIK.solver.axis = vector3;
           }  

           if (AimIK.solver.axis.x!= 0)
           {
               float angle = AimIK.solver.axis.x;
               angle = angle > 0 ?Mathf.Clamp(angle - horInput * inputspeed, 0,1.09f) :Mathf.Clamp(angle + horInput * 0.05f,-1.29f,0);
               Vector3 vector = AimIK.solver.axis;
               vector.x = angle;
               AimIK.solver.axis = vector;
           }
           */
            #endregion
        }
        if (_verInput != 0)
        {            
            _rotX = (angleX > _cemearVer.y && _verInput < 0) || (angleX < _cemearVer.x && _verInput > 0) ? _rotX : _rotX - _verInput * rotSpeed;
            _CamearTarget_rotX= (angleX > _cemearVer.y && _verInput < 0) || (angleX < _cemearVer.x && _verInput > 0) ? _CamearTarget_rotX : _CamearTarget_rotX - _verInput * rotSpeed;
            #region
            /*
            Roatet.y = (angleX > _VerRotateConstrainAangle.y && _verInput < 0) || (angleX < _VerRotateConstrainAangle.x && _verInput > 0) ? _verInput :0;
            float inputX = Mathf.Abs(_verInput);            
            if (_verInput > 0)
            {               
                float axisangle = AimIK.solver.axis.z;
                axisangle = Mathf.Clamp(axisangle - inputX * inputspeed, -0.67f, 1.84f);
                Vector3 axis = AimIK.solver.axis;
                axis.z = axisangle;
                AimIK.solver.axis = axis;
            }
            else
            {
                float axisangle = AimIK.solver.axis.z;
                axisangle = Mathf.Clamp(axisangle + inputX * inputspeed, 0.19f, 1.09f);
                Vector3 axis = AimIK.solver.axis;
                axis.z = axisangle;
                AimIK.solver.axis = axis;
            }
            */
            #endregion
        }      
        //_cam.SetHorVer(Roatet);
        if (_horInput != 0 || _verInput != 0)
        {
            Quaternion rotation = Quaternion.Euler(_rotX, -_rotY, 0);
            Quaternion camearTarget= Quaternion.Euler(_CamearTarget_rotX, -_CamearTarget_rotY, 0);
            // Quaternion.Euler(0, Y, 0);   基于Y轴的旋转 水平旋转
            // Quaternion.Euler(X, 0, 0);   基于X轴的旋转 垂直旋转
            //一个向量乘以一个四元数（使用Quaternion.Euler方法经选装角度转化为一个四元数，结果是基于旋转偏移位置，得到的是向量基于旋转的偏移位置           
            Target.position = playertarget.position - (camearTarget * _roundOffset);
            Target.LookAt(playertarget.position);
            transform.position = Target.position - (rotation * _offset);          
            transform.LookAt(Target.position);
        }
        /*
        float playerangleX = NormalizeAngle(_platerRoot.transform.eulerAngles.y);
        if (angleY >= _cemearHor.y && playerangleX < _HorRotateConstrainAangle.y)
        {
            playerangleX = Mathf.Clamp(playerangleX + _horInput, 0, _HorRotateConstrainAangle.y);
        }
        else if (angleY <= _cemearHor.x && playerangleX > _HorRotateConstrainAangle.x)
        {
            playerangleX = Mathf.Clamp(playerangleX - _horInput, 0, _HorRotateConstrainAangle.y);
        }
        _platerRoot.rotation = Quaternion.AngleAxis(playerangleX,Vector2.up);
        */
        #region
        /*
        float angle_x = Mathf.Abs(Mathf.Abs(angleX));
        float anglem_x = Mathf.Abs(NormalizeAngle(this.transform.localEulerAngles.x));
        float anglemm_x = Mathf.Abs(angleX > 0 ? _cemearVer.y : _cemearVer.x);
        //_verRatio = Mathf.Abs(angle_x - anglem_x) / anglemm_x;
        Debug.LogError(_horRatio);
        //_verRatio = 1;
        _horRatio = 1;// Mathf.Abs(Mathf.Abs(angleY) - Mathf.Abs(NormalizeAngle(this.transform.localEulerAngles.y))) / angleY > 0 ? _cemearHor.y : _cemearHor.x;
        */
        #endregion
        _cam.SetHorVer(new Vector2(_horInput, _verInput));
    }
    public void PlayerCamearReset() 
    {

        this.transform.DOLocalMove(_GunCamrar,0.5f);
        this.transform.DOLocalRotate(Vector3.zero, 0.5f);
        Target.DOLocalMove(_PlayerCapsule,0.5f);
        this.transform.DOLookAt(Target.position,0.5f);
        Target.DOLocalRotate(Vector3.zero, 0.5f);
        /*
        Vector3 vector = this.transform.position;
        Vector3 rotate = new Vector3(NormalizeAngle(this.transform.eulerAngles.x), NormalizeAngle(this.transform.eulerAngles.y), 0);
        while (this.transform.position!=_GunCamrar)
        {
            Vector3 vector3 = Vector3.Lerp(vector, _GunCamrar, 0.5F);
            this.transform.position = vector3;
            Vector3 GunCamearRotate = Vector3.Lerp(rotate, Vector3.zero, 0.5F);
            this.transform.eulerAngles = GunCamearRotate;

        } 
        */
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