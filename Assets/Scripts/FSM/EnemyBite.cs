using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
    public class Bite : StateBasic
    {
        Animator Animator;
        public Bite(int index, Animator animator, EnemyMobile aniBasic) : base(index, aniBasic)
        {
            this.Animator = animator;
        }

        public override void OnStart()
        {
            Animator.SetTrigger(Constant._bite_aniHas);
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