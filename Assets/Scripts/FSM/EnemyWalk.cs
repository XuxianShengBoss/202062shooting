using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
    public class Walk : StateBasic
    {
        Animator Animator;
        public Walk(int index, Animator animator, EnemyMobile aniBasic) : base(index, aniBasic)
        {
            this.Animator = animator;
        }

        public override void OnStart()
        {
            //_aniBasic.m_EnemyController.m_NavMeshAgent.velocity.magnitude
            Animator.SetFloat(Constant._IdleWalkRun_aniHas,0.5f);
        }

        public override void OnUpdate()
        {
            /*
            AnimatorStateInfo StateInfo;
            StateInfo = Animator.GetCurrentAnimatorStateInfo(0);        
            if (StateInfo.IsName(_aniBasic._attack)) 
            {
                if (StateInfo.normalizedTime >= 0.9f)
                    _IsPlayAni = true;
            }
            */
        }

        public override void OnExit()
        {
            _IsPlayAni = false;
        }

    }
}