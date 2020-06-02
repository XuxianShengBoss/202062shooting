using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyPatrolPath:MonoBehaviour
{
    public NavMeshAgent _nov;
    public float _moveSpeed;
    public float _PatroRadius;
    private Vector3[] _partroPath=new Vector3[2];
    private int _pathindex;
    public Vector3 _nextpos;
    void Start()
    {
        _nov=transform.GetComponent<NavMeshAgent>();
        _partroPath[1]=this.transform.position+this.transform.forward*_PatroRadius;
        _partroPath[0]=this.transform.position;
        _nextpos=_partroPath[1];
        _pathindex=0;
    }
    public void m_Update()
    {
        Debug.LogError(Vector3.Distance(_nextpos,this.transform.position));
        if(Vector3.Distance(_nextpos,this.transform.position)<=0.99f)
        {    
            _pathindex++;
            if(_pathindex>1)
            _pathindex=0;
            _nextpos=_partroPath[_pathindex];
        }
        _nov.speed=_moveSpeed;
        _nov.SetDestination(_nextpos);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position,_PatroRadius);
        //Debug.LogError(Vector3.Distance(new Vector3(2.4f, 3.3f, -14.7f),this.transform.position));
    }
}
