using UnityEngine;
using System.Collections;
using Pool;
using ClassEnum;
public class BulletScript : ObjRelaseBasic {

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	private float destroyAfter=1.5f;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;


	void OnEnable()
	{
		StartCoroutine ("DestroyAfter");
	}

	private void OnCollisionEnter (Collision collision) 
	{
		#region 
		/*
		if (collision.gameObject.tag == "Player") 
		{
			Debug.LogWarning("Collides with player");
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			
		}

		if (collision.transform.tag == "Blood") 
		{
			Instantiate (bloodImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
		  destroyOnImpact=true;
		}

		if (collision.transform.tag == Tag.Metal) 
		{
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
		    destroyOnImpact=true;
		}

		if (collision.transform.tag == "Dirt") 
		{
			Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			  destroyOnImpact=true;
		}
		if (collision.transform.tag == "Concrete") 
		{
			Instantiate (concreteImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			  destroyOnImpact=true;
		}

		if (collision.transform.tag == "Target") 
		{
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
		  destroyOnImpact=true;
		}
			
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
			  destroyOnImpact=true;
		}

		if (collision.transform.tag == "GasTank") 
		{
			collision.transform.gameObject.GetComponent
				<GasTankScript> ().isHit = true;
			  destroyOnImpact=true;
		}
		*/
        #endregion
		if (collision.transform.tag ==Tag.Enemy)
		{
			collision.transform.GetComponentInParent
				<Damageable>().InflictDamage(10,true,FindObjectOfType<PlayerCollider>().gameObject);
            /*
		      var bloodEffects=Instantiate(bloodImpactPrefabs[Random.Range
			(0, bloodImpactPrefabs.Length)],transform.position,
		 	Quaternion.LookRotation(collision.contacts[0].normal), collision.transform);
		     destroyOnImpact=true;
             */
			this.gameObject.SetActive(false);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		yield return new WaitForSeconds(3);
		//(Random.Range(minDestroyTime, maxDestroyTime));
        Relase();
		
	}

	private IEnumerator DestroyAfter () 
	{
		yield return new WaitForSeconds (destroyAfter);
		  Relase();
	}

	protected override void Relase()
	{
	   destroyOnImpact=false;
	   base.Relase();
	}
}