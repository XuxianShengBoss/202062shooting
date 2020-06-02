using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm {
public class Attack : StateBasic
    {
        Animator Animator;
        public Attack(int index, Animator animator, EnemyMobile aniBasic) : base(index, aniBasic)
        {
            this.Animator = animator;
        }

        public override void OnStart()
        {
            Animator.SetTrigger(Constant._attack_aniHas);
            _startTime = Time.time;
        }

        public override void OnUpdate()
        {
            if (Time.time >= _startTime + _aniBasic._Attack_time)
            {
                Animator.SetTrigger(Constant._attack_aniHas);
                _startTime = Time.time;
            }
        }

        public override void OnExit()
        {
            _IsPlayAni = false;
        }

    }
}