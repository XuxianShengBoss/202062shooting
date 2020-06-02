using System.Collections.Generic;
using UnityEngine;
using Fsm;

//动画状态跳转
[RequireComponent(typeof(EnemyController))]
public class EnemyMobile : MonoBehaviour
{
    public enum AIState
    {
        Patrol,//巡逻
        Follow,//追赶
        Attack,
        Idle,
        Damage,
        WaltAttack,
        Crawl,
        Jump,
        Death
    }


    //伤害特效
    public AudioClip MovementSound;
    private AudioSource m_AudioSource;
    protected Animator _ani;
    public MinMaxFloat PitchDistortionMovementSpeed;
    public AIState aiState;
    public EnemyAniState _defaultAiState;
    public EnemyController m_EnemyController;

    public float _Attack_time = 10f;
    public float _MoveSpeed = 1.5f;
    public float _waleSpeed = 0.2f;
    //攻击距离 
    public float attackStopDistanceRatio = 0.5f;

    private bool _isDamge;

    FSMMananger _fSM;

    public AIState _startState { set => this._startState = value; }

    public void Info()
    {
        _ani = GetComponentInChildren<Animator>();
        m_EnemyController = GetComponent<EnemyController>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = MovementSound;
        m_AudioSource.Play();
        _isDamge = false;
        aiState = AIState.Idle;
        m_EnemyController.onAttack += OnAttack;
        m_EnemyController.onDetectedTarget += OnDetectedTarget;
        m_EnemyController.onLostTarget += OnLostTarget;
        //m_EnemyController.SetPathDestinationToClosestNode();
        m_EnemyController.onDamaged += OnDamaged;
        m_EnemyController.m_Health.onDie += OnDeath;
        AddAniState();
    }
    void Start()
    {
        Info();
        Active();
    }

    public void Active()
    {
        if (aiState == AIState.Jump)
            _ani.SetTrigger(Constant._Jump_aniHas);
        else if(aiState== AIState.Crawl)
            _ani.SetTrigger(Constant._Crawl_aniHas);
    }

   public void m_Update()
   {
        UpdateAIStateTransitions();
        UpdateCurrentAIState();
        UpdateAniSatate();
   }

   public void Update()
   {
       if(Input.GetKeyDown(KeyCode.A))
       OnDamaged();
        UpdateAIStateTransitions();
        UpdateCurrentAIState();
        UpdateAniSatate();
   }
    void UpdateAniSatate()
    {
        switch (aiState)
        {
            case AIState.Patrol:
            case AIState.Follow:
                m_EnemyController.m_NavMeshAgent.speed = _defaultAiState == EnemyAniState.Move ? _MoveSpeed : _waleSpeed;
                _fSM.CutState(_defaultAiState);
                break;
            case AIState.Attack:
                _fSM.CutState(EnemyAniState.Attack);
                break;
            case AIState.Idle:
                _fSM.CutState(EnemyAniState.Idle);
                break;
            case AIState.Damage:
                m_EnemyController.m_NavMeshAgent.speed = 0;
                _fSM.CutState(EnemyAniState.Damage);
                break;
            case AIState.WaltAttack:
                _fSM.CutState(EnemyAniState.Walk);
                break;
            default:
                break;
        }
        _fSM.Update();
    }

    void UpdateAIStateTransitions()
    {
        switch (aiState)
        {

            case AIState.Idle:
            case AIState.Patrol:
                if (m_EnemyController.isSeeingTarget)
                    aiState = AIState.Follow;
                if (_isDamge)
                {
                    _isDamge = false;
                    aiState = AIState.Damage;
                }
                break;
            case AIState.Follow:
                //当目标有视线时，转换为攻击
                if (m_EnemyController.isTargetInAttackRange)
                {
                    aiState = AIState.Attack;
                    m_EnemyController.SetNavDestination(transform.position);
                }
                break;
            case AIState.Attack:
                //当目标不在攻击范围内时，转换为跟随
                if (!m_EnemyController.isTargetInAttackRange)
                {
                    aiState = AIState.Idle;
                }
                break;
        }
    }

    void UpdateCurrentAIState()
    {
        switch (aiState)
        {
            case AIState.Patrol://巡逻
                m_EnemyController.UpdatePathDestination();
                break;
            case AIState.Follow://跟随
                m_EnemyController.SetNavDestination(m_EnemyController.knownDetectedTarget.transform.position);
                m_EnemyController.OrientTowards(m_EnemyController.knownDetectedTarget.transform.position);
                break;
            case AIState.Attack://攻击

                if (Vector3.Distance(m_EnemyController.knownDetectedTarget.transform.position,
                                    m_EnemyController.transform.position)
                                    >= (attackStopDistanceRatio * m_EnemyController.attackRange))
                {
                    m_EnemyController.SetNavDestination(m_EnemyController.knownDetectedTarget.transform.position);
                }
                else
                {
                    m_EnemyController.SetNavDestination(transform.position);
                }
                m_EnemyController.OrientTowards(m_EnemyController.knownDetectedTarget.transform.position);
                //m_EnemyController.TryAtack((m_EnemyController.knownDetectedTarget.transform.position - m_EnemyController.weapon.transform.position).normalized);
                break;
        }
    }

    void OnAttack()
    {
    }

    void OnDetectedTarget()
    {
        if (aiState == AIState.Patrol)
        {
            aiState = AIState.Follow;
        }
        /*
        for (int i = 0; i < onDetectVFX.Length; i++)
        {
            onDetectVFX[i].Play();
        }

        if (onDetectSFX)
        {
            AudioUtility.CreateSFX(onDetectSFX, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
        }
        */
    }

    void OnLostTarget()
    {
        if (aiState == AIState.Follow 
         || aiState == AIState.Attack)
        {
            aiState = AIState.Patrol;
        }
    }

    void OnDamaged()
    {
        /*
        if (randomHitSparks.Length > 0)
        {
            int n = Random.Range(0, randomHitSparks.Length - 1);
            randomHitSparks[n].Play();
        }
        */
        if (_fSM._currentFsmState == EnemyAniState.Attack)
            return;
        this.aiState = AIState.Idle;
        _isDamge = true;
        m_EnemyController.m_NavMeshAgent.speed = 0;
    }

    public void OnDeath()
    {
        aiState = AIState.Death;
        _fSM.CutState( EnemyAniState.Die);
    }

    void AddAniState()
    {
        _fSM = new FSMMananger();
        _fSM.AddAniState(_ani,this);
    }

}
