using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
public class Damage : StateBasic
{
    Animator Animator;
    public Damage(int index,Animator animator,EnemyMobile aniBasic) : base(index, aniBasic)
    {
        this.Animator = animator;
    }
        float wait = 1.5f;
        float time;
    public override void OnStart()
    {
      Animator.SetTrigger(Constant._damage_aniHas);
            time = Time.time;
    }

    public override void OnUpdate()
    {
            if (Time.time >= wait + time)
            {
               _aniBasic.aiState = EnemyMobile.AIState.Idle;
            }
        /*
        AnimatorStateInfo StateInfo;
        StateInfo = Animator.GetCurrentAnimatorStateInfo(0);        
        if (StateInfo.IsName("Damage")) 
        {
            if (StateInfo.normalizedTime >= 0.99f)
                _aniBasic.aiState = EnemyMobile.AIState.Idle;
        }
        */
    }

    public override void OnExit()
    {
        _IsPlayAni = false;
    }
}
}
