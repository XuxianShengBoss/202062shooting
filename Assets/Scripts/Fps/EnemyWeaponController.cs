using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyWeaponController : WeaponController
{
    protected override  void Update(){}
    protected override void HandleShoot (){}

    protected override bool TryShoot()
    {
        if (m_CurrentAmmo >= 1f 
            && m_LastTimeShot + delayBetweenShots < Time.time)
        {
            return true;
        }
        return false;
    }
}
