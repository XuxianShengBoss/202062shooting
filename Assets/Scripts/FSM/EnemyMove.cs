using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
    public class EnemyMove : StateBasic
    {
        Animator Animator;
        EnemyController EnemyController;
        AudioSource AudioSource;
        public EnemyMove(int index, Animator animator, EnemyMobile aniBasic) : base(index, aniBasic)
        {
            this.Animator = animator;
            _aniBasic = aniBasic;
            EnemyController = _aniBasic.m_EnemyController;
            AudioSource = _aniBasic.transform.GetComponent<AudioSource>();
            AudioSource.clip = _aniBasic.MovementSound;
            AudioSource.pitch = 0;
            AudioSource.Play();
        }

        public override void OnStart()
        {
            Animator.SetFloat(Constant._IdleWalkRun_aniHas,_aniBasic.m_EnemyController.m_NavMeshAgent.velocity.magnitude);
            _startTime = Time.time;
        }

        public override void OnUpdate()
        {
            float movespeed = EnemyController.m_NavMeshAgent.velocity.magnitude;
            float pitch = Mathf.Clamp(EnemyController.m_NavMeshAgent.speed, 0, 2.5f);
            Animator.SetFloat(Constant._IdleWalkRun_aniHas, movespeed);
            AudioSource.pitch = Mathf.Lerp(AudioSource.pitch, pitch, movespeed / EnemyController.m_NavMeshAgent.speed);
            /*
            if (Time.time >= _startTime + Animator.GetCurrentAnimatorStateInfo(0).length)
            {
                _startTime = Time.time;
                Animator.SetTrigger(Constant._move_aniHas);
            }
            */
        }

        public override void OnExit()
        {
            _IsPlayAni = false;
        }

    }
}