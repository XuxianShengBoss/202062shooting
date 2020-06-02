using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
    public class EnemyIdle : StateBasic
    {
        Animator Animator;
        public EnemyIdle(int index, Animator animator, EnemyMobile basic) : base(index, basic)
        {
            this.Animator = animator;
        }

        public override void OnStart()
        {
            this._IsPlayAni = false;
            Animator.SetFloat(Constant._IdleWalkRun_aniHas,0);
        }
        public override void OnExit()
        {
            this._IsPlayAni = false;
        }
    }
}