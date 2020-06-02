using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ClassEnum
{
    public enum PlayerState
    {
        Idel,
        Run,
        Die,
        Reload,
        HuanQiang,
        Sniper,
        Ani_SnipeUp,
        Shooting,
        Ani_SnipeBack,
        One
    }


 public enum EnemyType
 {
  Robot,
  Zombies
 }

 public enum PoolPrefabType
 {
        One,
        BloodImpact,
        ConcreteImpact,
        DirtImpact,
        MetalImpact,
        Bullet_Prefab,
        Small_Casing_Prefab,
        Hand_Grenade_Prefab
}

 public struct WaveObj
 {
        public int Level;
        public List<EnemyController> enemyControllers;
 }
}

