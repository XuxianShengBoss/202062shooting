using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fsm
{
    public abstract class StateBasic
    {
        protected EnemyMobile _aniBasic;
        public StateBasic(int index, EnemyMobile aniBasic)
        {
            _stateIndex = index;
            _IsPlayAni = false;
            this._aniBasic = aniBasic;
        }
        public int _stateIndex;
        protected bool _IsPlayAni { set; private get; }
        protected float _anitime;
        protected float _startTime;
        public virtual void OnStart() { }
        public virtual void OnExit() { }
        public virtual void OnReset() { }
        public virtual void OnUpdate() { }
        public bool GetIsPlayerAni()
        {
            return _IsPlayAni;
        }
    }
}