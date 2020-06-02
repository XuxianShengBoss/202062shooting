using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ClassEnum;
using Pool;
using RootMotion.FinalIK;

public class AutomaticGunScriptLPFP : GunBasic {

	Animator anim;
	public Camera gunCamera;
	public float fovSpeed = 15.0f;
	public float defaultFov = 40.0f;
	public bool scope1;
	[Range(5, 40)]
	public float scope1AimFOV = 10;
	[Header("Weapon Sway")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;

	private float lastFired;
	public float fireRate;
	public bool autoReload;
	//换弹时间
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	//Holstering weapon
	private bool hasBeenHolstered = false;
	//If weapon is holstered
	private bool holstered;
	//Check if running
	private bool isRunning;
	//Check if aiming
	private bool isAiming;
	//Check if walking
	private bool isWalking;
	//Check if inspecting weapon
	private bool isInspecting;

	//How much ammo is currently left
	private int currentAmmo;
	//Totalt amount of ammo
	[Tooltip("How much ammo the weapon should have.")]
	public int ammo;
	//Check if out of ammo
	private bool outOfAmmo;
	public float bulletForce = 400.0f;
	public float showBulletInMagDelay = 0.6f;
	public float grenadeSpawnDelay = 0.35f;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;
	public float lightDuration = 0.02f;

	[System.Serializable]
	public class prefabs
	{
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;

	[System.Serializable]
	public class spawnpoints
	{
		public Transform casingSpawnPoint;//子弹生成点
		public Transform bulletSpawnPoint;//弹壳生成点
		public Transform grenadeSpawnPoint;//手榴弹生成点
	}
	

	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip silencerShootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;
	public spawnpoints Spawnpoints;
	public float magnitude = 0.5f;
	public float _bulletMass=10f;
	private Recoil recoil;

	private bool soundHasPlayed = false;
	//======================>>>>>>>>>>> GunData

	private void Awake() {
		anim = GetComponent<Animator>();
		currentAmmo = ammo;
	}

	public override void Start() {
		base.Start();
		recoil = GetComponent<Recoil>();
		//GameContorlManager._Instance._uiContorlManager._OnShot += OnShooting;
		GameContorlManager._Instance._uiContorlManager._OnHunDan += Reload;
		GameContorlManager._Instance._uiContorlManager._OnSniper += OpenrorCloseSniper;
		initialSwayPosition = transform.localPosition;
		InIt();
		
	}

	private void InIt()
    {
		BulletCount = ammo;
		autoReloadDelay = 1.25f;
		EventManager.Broadcast<int>(UIButtonEvent.SetBulletCountText,BulletCount);
		outOfAmmo=false;
	}

	private void LateUpdate() {

		return;
		//Weapon sway
		if (weaponSway == true)
		{
			float movementX = -Input.GetAxis("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
			//Clamp movement to min and max values
			movementX = Mathf.Clamp
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp
				(movementY, -maxSwayAmount, maxSwayAmount);
			//Lerp local pos
			Vector3 finalSwayPosition = new Vector3
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp
				(transform.localPosition, finalSwayPosition +
					initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}

	private void Update() {

		if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
		{
			OnShooting();
				
		}
		#region//开启望远镜
		/*
		 gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
	     defaultFov, fovSpeed * Time.deltaTime);
		*/
		#endregion
    }

    private IEnumerator GrenadeSpawnDelay() {

		//Wait for set amount of time before spawning grenade
		yield return new WaitForSeconds(grenadeSpawnDelay);
		//Spawn grenade prefab at spawnpoint
		Instantiate(Prefabs.grenadePrefab,
			Spawnpoints.grenadeSpawnPoint.transform.position,
			Spawnpoints.grenadeSpawnPoint.transform.rotation);
	}

    //============Contorl=========>>>>> Shooting Huandan
	public void OnShooting()
	{
		if (GetIsCutState) return;
		//是否在装弹药  弹药是否足够 射击事件是否达到    
		if (BulletCount > 0)
		{
			if (Time.time - lastFired > 1 / fireRate)
			{
				lastFired = Time.time;
				BulletCount -= 1;
				if (!isAiming)
				{
					_fsm.CutState(PlayerState.Shooting);
				}
				
				RaycastHit raycastHit;
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
				Debug.DrawLine(ray.origin, ray.direction * 1000, Color.red);
				Vector3 point = ray.direction.normalized * 400;
				if (Physics.Raycast(ray, out raycastHit))
					point = raycastHit.point;
				var bullet = PoolManager.Get.GetObj(PoolPrefabType.Bullet_Prefab, null, Spawnpoints.bulletSpawnPoint.transform.position, Spawnpoints.bulletSpawnPoint.transform.rotation);				
				Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
                rigidbody.velocity = (point-this.transform.localPosition).normalized* bulletForce;
				recoil.Fire(magnitude);
				var casingSpawnobj=PoolManager.Get.GetObj(PoolPrefabType.Small_Casing_Prefab,null,Spawnpoints.casingSpawnPoint.position,Spawnpoints.casingSpawnPoint.transform.rotation);
				/*
				Instantiate(Prefabs.casingPrefab,
					Spawnpoints.casingSpawnPoint.transform.position,
					Spawnpoints.casingSpawnPoint.transform.rotation);
				*/
				if (BulletCount <= 0&&!outOfAmmo)
				{
					outOfAmmo = true;
					Reload();
				}
				EventManager.Broadcast<int>(UIButtonEvent.SetBulletCountText, BulletCount);
			}
		}
	}

	private void Reload()
	{
		if (_fsm._currstate == PlayerState.Reload) return;
		Debug.LogError(_fsm._currstate);
		StartCoroutine(AutoReload());
	}

	public void OpenrorCloseSniper(bool opneorclose)
    {
		if (GetIsCutState) return;
		_fsm.CutState(opneorclose&& _fsm._currstate!= PlayerState.Ani_SnipeUp?PlayerState.Ani_SnipeUp: PlayerState.Ani_SnipeBack);
    }

	private IEnumerator AutoReload()
	{
		if (_fsm._currstate == PlayerState.Ani_SnipeUp)
        {
			OpenrorCloseSniper(false);
			yield return new WaitUntil(()=>((WaitAni)(_fsm._CurrState)).IsExit);
        }
		outOfAmmo = false;
		_fsm.CutState(PlayerState.Reload);
		yield return new WaitForSeconds(autoReloadDelay);
		StartCoroutine(ShowBulletInMag());
		currentAmmo = ammo;
		outOfAmmo = false;
	}

    public bool GetIsCutState { get => _fsm._currstate == PlayerState.HuanQiang || _fsm._currstate == PlayerState.Reload; }

	private IEnumerator ShowBulletInMag()
	{
		yield return new WaitForSeconds(showBulletInMagDelay);
		BulletCount = ammo;
		EventManager.Broadcast<int>(UIButtonEvent.SetBulletCountText,BulletCount);
	}
}