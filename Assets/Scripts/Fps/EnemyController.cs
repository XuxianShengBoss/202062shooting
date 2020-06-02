using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using ClassEnum;
using RootMotion.Dynamics;

//控制器
[RequireComponent(typeof(Health), typeof(Actor), typeof(NavMeshAgent))]
public class EnemyController : EnemyBasic
{
    [Tooltip("The Y height at which the enemy will be automatically killed (if it falls off of the level)")]
    public float selfDestructYHeight = -20f;
    [Tooltip("The speed at which the enemy rotates")]
    public float orientationSpeed = 10f;

    [Tooltip("The max distance at which the enemy can attack its target")]
    public float attackRange = 10f;
    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onDamaged;
    public UnityAction onLostTarget;

    //检测到的目标
    public GameObject knownDetectedTarget { get; private set; }  
    public bool isTargetInAttackRange { get; private set; }
    public bool isSeeingTarget { get; private set; }
    public bool hadKnownTarget { get; private set; }
    public NavMeshAgent m_NavMeshAgent { get; private set; }
    //private PatrolPath patrolPath;
    private EnemyManager m_EnemyManager;
    private ActorsManager m_ActorsManager;
    private PuppetMaster m_PuppetMaster;
    [HideInInspector]
    public EnemyMobile m_EnemyMobile;
    private EnemyPatrolPath m_EnemyPatrolPath;
    [HideInInspector]
    public Health m_Health;
    Actor m_Actor;
    float m_TimeLastSeenTarget = Mathf.NegativeInfinity;
    Collider[] m_SelfColliders;
    //GameFlowManager m_GameFlowManager;
    bool m_WasDamagedThisFrame;

void Awake()
{
    info();
}
    public void info()
    {
        m_Health = GetComponent<Health>();
        m_Actor = GetComponent<Actor>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();     
        m_EnemyMobile = this.transform.GetComponent<EnemyMobile>();
        m_EnemyPatrolPath=transform.GetComponent<EnemyPatrolPath>();
        m_Health=this.transform.GetComponent<Health>();
        m_PuppetMaster=transform.GetComponentInChildren<PuppetMaster>();
        //m_ZombiesData = MyGame.zombiesDatas.Find(f => f.ZombiesID == _ID);
        m_Health.onDie += OnDie;
        m_Health.onDamaged += OnDamaged;
        //weapon.owner = gameObject;
        //m_PuppetMaster.state = PuppetMaster.State.Alive;
        m_EnemyMobile.Info();
    }
    void Start()
    {
        m_EnemyManager = FindObjectOfType<EnemyManager>();      
        m_ActorsManager = FindObjectOfType<ActorsManager>();
        m_EnemyManager.RegisterEnemy(this);
    }

    public void m_Update()
    {
        if (m_Health.m_IsDead) return;
        //EnsureIsWithinLevelBounds();
        //HandleTargetDetection();
        m_EnemyMobile.m_Update();
        //m_WasDamagedThisFrame = false;
    }
    /*
    void EnsureIsWithinLevelBounds()
    {
        if (transform.position.y < selfDestructYHeight)
        {
            Destroy(gameObject);
            return;
        }
    }

    void HandleTargetDetection()
    {
        
        //处理已知的目标检测超时
        if (knownDetectedTarget 
        && !isSeeingTarget 
        && (Time.time - m_TimeLastSeenTarget) > knownTargetTimeout)
        {
            knownDetectedTarget = null;
        }
        //找到最接近的敌人
        float sqrDetectionRange = detectionRange * detectionRange;
        isSeeingTarget = false;
        float closestSqrDistance = Mathf.Infinity;
        foreach (Actor actor in m_ActorsManager.actors)
        {
            if (actor.affiliation != m_Actor.affiliation)
            {
                float sqrDistance = (actor.transform.position - detectionSourcePoint.position).sqrMagnitude;
                if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                {
                    // 检查是否有障碍物
                    RaycastHit[] hits = Physics.RaycastAll(detectionSourcePoint.position, (actor.aimPoint.position - detectionSourcePoint.position).normalized, detectionRange, -1, QueryTriggerInteraction.Ignore);
                    RaycastHit closestValidHit = new RaycastHit();
                    closestValidHit.distance = Mathf.Infinity;
                    bool foundValidHit = false;
                    foreach (var h in hits)
                    {
                        if(!m_SelfColliders.Contains(h.collider) 
                           && h.distance < closestValidHit.distance)
                        {
                            closestValidHit = h;
                            foundValidHit = true;
                        }
                    }

                    if(foundValidHit)
                    {
                        Actor hitActor = closestValidHit.collider.GetComponentInParent<Actor>();
                        if (hitActor == actor)
                        {
                            isSeeingTarget = true;
                            closestSqrDistance = sqrDistance;
                            m_TimeLastSeenTarget = Time.time;
                            knownDetectedTarget = actor.aimPoint.gameObject;
                        }
                    }
                }
            }
        }
        isTargetInAttackRange = knownDetectedTarget != null && Vector3.Distance(transform.position, knownDetectedTarget.transform.position) <= attackRange;
        hadKnownTarget = knownDetectedTarget != null;
    
    }
    */
    public void OrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * orientationSpeed);
        }
    }

/*
    private bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    public void SetPathDestinationToClosestNode()
    {
        if (IsPathValid())
        {
            int closestPathNodeIndex = 0;
            for (int i = 0; i < patrolPath.pathNodes.Count; i++)
            {
                float distanceToPathNode = patrolPath.GetDistanceToNode(transform.position, i);
                if (distanceToPathNode < patrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                {
                    closestPathNodeIndex = i;
                }
            }

            m_PathDestinationNodeIndex = closestPathNodeIndex;
        }
        else
        {
            m_PathDestinationNodeIndex = 0;
        }
    }

    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return patrolPath.GetPositionOfPathNode(m_PathDestinationNodeIndex);
        }
        else
        {
            return transform.position;
        }
    }
    */

    public void SetNavDestination(Vector3 destination)
    {
        if (m_NavMeshAgent)
        {
            m_NavMeshAgent.SetDestination(destination);
        }
    }

    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if(knownDetectedTarget==null)
         m_EnemyPatrolPath.m_Update();
        /*
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= pathReachingRadius)
            {
                // increment path destination index
                m_PathDestinationNodeIndex = inverseOrder ? (m_PathDestinationNodeIndex - 1) : (m_PathDestinationNodeIndex + 1);
                if (m_PathDestinationNodeIndex < 0)
                {
                    m_PathDestinationNodeIndex += patrolPath.pathNodes.Count;
                }
                if (m_PathDestinationNodeIndex >= patrolPath.pathNodes.Count)
                {
                    m_PathDestinationNodeIndex -= patrolPath.pathNodes.Count;
                }
            }
        }
        */
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // test if the damage source is the player
        if (damageSource && damageSource.GetComponent<PlayerCollider>())
        {
            // pursue the player
            m_TimeLastSeenTarget = Time.time;
            knownDetectedTarget = damageSource;

            if (onDamaged != null)
            {
                onDamaged.Invoke();
            }
            /*
            m_LastTimeDamaged = Time.time;
            m_WasDamagedThisFrame = true;
            */
        }
    }

    void OnDie()
    {
        m_EnemyManager.UnregisterEnemy(this);
        m_PuppetMaster.state = PuppetMaster.State.Dead;
    }

    /*
    public bool TryAtack(Vector3 weaponForward)
    {
        return;
        //武器指向玩家
        weapon.transform.forward = weaponForward;
        bool didFire = weapon.HandleShootInputs(false, true, false);
        if(didFire && onAttack != null)
        {
            onAttack.Invoke();
        }
         return didFire;
    }
    */

    public bool TryDropItem()
    {
        return false;
        /*
        if (dropRate == 0 || lootPrefab == null)
            return false;
        else if (dropRate == 1)
            return true;
        else
            return (Random.value <= dropRate);
      */
    }
}
