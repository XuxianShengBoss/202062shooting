using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEnum;


public class GunBasic : MonoBehaviour
{
   protected Animator _ani;

    protected int BulletCount;
    [HideInInspector]
    public PlayerStataFsm _fsm;
    public virtual void Start()
    {
        _fsm= new PlayerStataFsm();
        this._ani = this.GetComponent<Animator>();
        _fsm.Inif(_ani);
    }

    public void CutAni(PlayerState state)
    {

    }

    public virtual void FixedUpdate()
    {
        _fsm.OnUpdate();
    }
}

public class PlayerStataFsm
{

    protected int Ani_Idel = Animator.StringToHash(Constant.Ani_Idel);
    protected int Ani_Reload = Animator.StringToHash(Constant.Ani_Reload);
    protected int Ani_Shooting = Animator.StringToHash(Constant.Ani_Shooting);
    protected int Ani_Run = Animator.StringToHash(Constant.Ani_Run);
    protected int Ani_HuanQiang = Animator.StringToHash(Constant.Ani_HuanQiang);
    protected int Ani_SnipeShooting = Animator.StringToHash(Constant.Ani_SnipeShooting);
    protected int Ani_SnipeUp = Animator.StringToHash(Constant.Ani_SnipeUp);
    protected int Ani_SnipeBack = Animator.StringToHash(Constant.Ani_SnipeBack);

    public void GetAaniHash()
    {
        Ani_Idel = Animator.StringToHash(Constant.Ani_Idel);
        Ani_Reload = Animator.StringToHash(Constant.Ani_Reload);
        Ani_Shooting = Animator.StringToHash(Constant.Ani_Shooting);
        Ani_Run = Animator.StringToHash(Constant.Ani_Run);
        Ani_HuanQiang = Animator.StringToHash(Constant.Ani_HuanQiang);
        Ani_SnipeShooting = Animator.StringToHash(Constant.Ani_SnipeShooting);
        Ani_SnipeUp = Animator.StringToHash(Constant.Ani_SnipeUp);
        Ani_SnipeBack = Animator.StringToHash(Constant.Ani_SnipeBack);
    }

    public PlayerState _currstate = PlayerState.One;
    protected PlayerState _waitani= PlayerState.One;
    public State _CurrState { get; set; }

    private Dictionary<PlayerState, State> stateDic = new Dictionary<PlayerState, State>();
    public void Inif(Animator ani)
    {
        GetAaniHash();
        stateDic.Add(PlayerState.Idel, new IdleAni(ani, Ani_Idel));
        stateDic.Add(PlayerState.HuanQiang, new WaitAni(ani, Ani_HuanQiang));
        stateDic.Add(PlayerState.Reload, new WaitAni(ani, Ani_Reload));
        stateDic.Add(PlayerState.Shooting, new IdleAni(ani, Ani_Shooting));
        stateDic.Add(PlayerState.Run, new IdleAni(ani, Ani_Run));
        stateDic.Add(PlayerState.Sniper, new IdleAni(ani, Ani_SnipeShooting));
        stateDic.Add(PlayerState.Ani_SnipeUp, new IdleAni(ani, Ani_SnipeUp));
        stateDic.Add(PlayerState.Ani_SnipeBack, new WaitAni(ani, Ani_SnipeBack));
    }
    public void CutState(PlayerState playerState)
    {
        if (_currstate == PlayerState.HuanQiang &&
            _currstate == PlayerState.Reload)
        {
            _waitani = playerState;
            return;
        }

        if (_currstate == playerState)
        {
            _CurrState.Start();
            return;
        }
        if (_currstate == PlayerState.Ani_SnipeUp&&playerState== PlayerState.Shooting)
        {
            stateDic[PlayerState.Sniper].Start();
            return;
        }
        if (_CurrState != null)
            _CurrState.Exit();
        State state = stateDic[playerState];
        if (state == null)
            Debug.LogError(string.Format("当前动画==>{0},切换动画==>{1}", _waitani.ToString(), playerState.ToString()));
        else
        {
            _currstate = playerState;
            _CurrState = state;
            _CurrState.Start();
        }
    }
    public void WaitStateCut()
    {

    }
    public void OnUpdate()
    {
        if (_CurrState != null)
            _CurrState.Update();
        if (_CurrState != null&&_currstate== PlayerState.Reload&&((WaitAni)_CurrState).IsExit)
        {
            _CurrState = null;
            _currstate =  PlayerState.One;
            _waitani = PlayerState.One;
            CutState(_waitani == PlayerState.One ? PlayerState.Idel : _waitani);
        }
    }
}


public class State
{
    public Animator _ani;
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}

public class WaitAni:State
{
    public bool IsExit=false;
    public int _hash;
    private float _StartTime;
    public WaitAni(Animator animator,int hash)
    {
        this._ani = animator;
        this._hash = hash;
    }

    public override void Start()
    {
        IsExit = false;
        base.Start();
        _ani.SetTrigger(_hash);
        _StartTime = Time.time;
    }
    public override void Update()
    {
        if((Time.time-_StartTime)>= _ani.GetCurrentAnimatorStateInfo(0).length&&!IsExit)
        {
           Exit();
        }
    }

    public override void Exit()
    {
        IsExit = true;
    }
}

public class IdleAni : State
{
    public int _hash;
    public IdleAni(Animator animator, int hash)
    {
        this._ani = animator;
        this._hash = hash;
    }

    public override void Start()
    {
        base.Start();
        _ani.SetTrigger(_hash);
    }
    public override void Update()
    {
    }

    public override void Exit()
    {
    
    }
}



