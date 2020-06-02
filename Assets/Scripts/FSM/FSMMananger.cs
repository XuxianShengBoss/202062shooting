using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsm
{
    public class FSMMananger
    {
        public FSMMananger()
        {
            StatesDic = new List<StateBasic>();
            _currentFsmState = EnemyAniState.One;
            _currstatebasic = null;
        }

        private List<StateBasic> StatesDic;
        public EnemyAniState _currentFsmState { get; set; }
        private StateBasic _currstatebasic;
        public void AddState(StateBasic stateBasic)
        {
            if (!StatesDic.Exists(f => f._stateIndex == stateBasic._stateIndex))
            {
                StatesDic.Add(stateBasic);
            }
        }

        public void CutState(EnemyAniState enemyAniState)
        {
            if (enemyAniState == EnemyAniState.Die)
            {
                _currentFsmState = EnemyAniState.One;
                _currstatebasic = null;
                return;
            }
            if (_currentFsmState == enemyAniState) return;
            StateBasic stateBasic = null;
            stateBasic = StatesDic.Find(f => f._stateIndex == (int)enemyAniState);
            if (stateBasic != null)
            {
                stateBasic.OnStart();
                _currentFsmState = enemyAniState;
                _currstatebasic = stateBasic;
            }
        }

        public bool GetAniState(EnemyAniState enemyAniState)
        {
            StateBasic state = null;
            state = StatesDic.Find(f => f._stateIndex == (int)enemyAniState);
            if (state != null)
                return state.GetIsPlayerAni();
            else Debug.LogError("未获取状态" + enemyAniState.ToString());
            return false;
        }

        public void Update()
        {
            if (_currstatebasic != null)
                _currstatebasic.OnUpdate();
        }

        public void AddAniState(Animator _ani, EnemyMobile aniBasic)
        {
            this.AddState(new Attack((int)EnemyAniState.Attack, _ani, aniBasic));
            this.AddState(new Bite((int)EnemyAniState.Bite, _ani, aniBasic));
            this.AddState(new Damage((int)EnemyAniState.Damage, _ani, aniBasic));
            this.AddState(new Walk((int)EnemyAniState.Walk, _ani, aniBasic));
            this.AddState(new EnemyMove((int)EnemyAniState.Move, _ani, aniBasic));
            this.AddState(new EnemyIdle((int)EnemyAniState.Idle, _ani, aniBasic));
        }
    }
}