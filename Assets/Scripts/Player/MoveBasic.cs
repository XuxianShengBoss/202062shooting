using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBasic : MonoBehaviour
{
   
    public PlayerCollider _PlayerCollider;
    public AniType m_AniType;
    public State m_state;
    private void Awake()
    {
        info();
    }
    public virtual void info(){}
    public virtual void m_Start()
    {
        if (m_AniType == AniType.Idel)
            _PlayerCollider.m_FPSCharacter.walkSpeed = 0;
        else 
        {
             _PlayerCollider._PlayerCamearConllider.PlayerCamearReset();
             m_state = State.Update;
        }
    }
    public virtual void M_Exit()
    {
        m_state = State.Succeed;
    }
    public virtual void m_Update()
    {
        if (m_state == State.Update) 
        {
            if (Vector3.Distance(this.transform.position, _PlayerCollider.transform.position) > 0.99f)
            {
                _PlayerCollider._nav.SetDestination(this.transform.position);
                _PlayerCollider._nav.speed = m_AniType == AniType.Walk ? Constant.PlayerWalkSpeed : Constant.PlayerMoveSpeed;
                _PlayerCollider.m_FPSCharacter.walkSpeed = m_AniType == AniType.Walk ? 0.5f : 1f;
            }
            else                      
                M_Exit();            
        }
    }
    public enum State
    {
        Succeed,
        Failure,
        Update
    }

    public enum AniType 
    {
       Move,
       Walk,
       Idel
    }
}
