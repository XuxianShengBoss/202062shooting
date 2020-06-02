using RootMotion.Demos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
/* 
*********************************************************************
Copyright (C) 2019 The Company Name
File Name:           PlayerCollider.cs
Author:              #AuthorName
CreateTime:          #CreateTime
********************************************************************* 
*/
public class PlayerCollider : MonoBehaviour
{    
    public float _RunSpeed;
    public float _WalkSpeed;
    [Header("移动速度lerp 时间")]
    public float movementSmoothness = 0.001f;
    public Rigidbody _rigidbody;
    public CapsuleCollider _collider;
    private SmoothRotation _rotationX;
    private SmoothRotation _rotationY;     
    private SmoothVelocity _velocityX;
    private SmoothVelocity _velocityZ;
    private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
    private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];
    public FPSCharacter m_FPSCharacter;
    public GunBasic _gunBasic;
    public NavMeshAgent _nav;
    public PlayerCamearConllider _PlayerCamearConllider;

    private void Awake()
    {
        GameContorlManager._Instance._playerCollider = this;    
    }

    private void Start()
    {
       // Info();
    }
    private void Info()
    {
        _velocityX = new SmoothVelocity();
        _velocityZ = new SmoothVelocity();
    }
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        return;
        var direction = new Vector3(Input.GetAxis(InputConstant.Horizontal), 0f,Input.GetAxis(InputConstant.Vertical)).normalized;
        var worldDirection = transform.TransformDirection(direction);
        var velocity = worldDirection * (GameContorlManager._Instance._playerCollider._gunBasic._fsm._currstate== ClassEnum.PlayerState.Run?_RunSpeed : _WalkSpeed);
        var intersectsWall = CheckCollisionsWithWalls(velocity);
        if (intersectsWall)
        {
            _velocityX.Current = _velocityZ.Current = 0f;
            return; 
        }
        var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
        var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
        var rigidbodyVelocity = _rigidbody.velocity;
        var force = new Vector3(smoothX - rigidbodyVelocity.x, 0f, smoothZ - rigidbodyVelocity.z);
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private bool CheckCollisionsWithWalls(Vector3 velocity)
    {
        return false;
        //if (_isGrounded) return false;
        var bounds = _collider.bounds;
        var radius = _collider.radius;
        var halfHeight = _collider.height * 0.5f - radius * 1.0f;
        var point1 = bounds.center;
        point1.y += halfHeight;
        var point2 = bounds.center;
        point2.y -= halfHeight;
        Physics.CapsuleCastNonAlloc(point1, point2, radius, velocity.normalized, _wallCastResults,
            radius * 0.04f, ~0, QueryTriggerInteraction.Ignore);
        var collides = _wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider);
        if (!collides) return false;
        for (var i = 0; i < _wallCastResults.Length; i++)
        {
            _wallCastResults[i] = new RaycastHit();
        }
        return true;
    }

    private class SmoothRotation
    {
        private float _current;
        private float _currentVelocity;

        public SmoothRotation(float startAngle)
        {
            _current = startAngle;
        }

        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }

        public float Current
        {
            set { _current = value; }
        }
    }

    private class SmoothVelocity
    {
        private float _current;
        private float _currentVelocity;
        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);
        }

        public float Current
        {
            set { _current = value; }
        }
    }
}
