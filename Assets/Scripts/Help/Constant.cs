using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant 
{
    public const string AssetPath = "/AssetData/AssetWave";
    public const string projectPath = "Assets/Datas";
    public const string zombiesPosPath = "/Zombies";
    public const string zombiesPosDataName = "/Scene{0}.Asset";
    public const string DussecretKey = "32434324";
    public const string DataName = "/Data";
    public const string AssetBundlePath= "Assets/Resources/AssetsBundle/";
    public static string SavePaht = Application.persistentDataPath;

    public const string Ani_Idel = "Idel";
    public const string Ani_Reload = "Reload";
    public const string Ani_Shooting = "Shooting";
    public const string Ani_Run = "Run";
    public const string Ani_HuanQiang = "HuanQiang";
    public const string Ani_SnipeShooting = "SnipeShooting";
    public const string Ani_SnipeUp = "SnipeUp";
    public const string Ani_SnipeBack = "SnipeBack";

    public static int _bite_aniHas = Animator.StringToHash("Bite");
    public static int _attack_aniHas = Animator.StringToHash("Attack");
    public static int _damage_aniHas = Animator.StringToHash("Damage");
    public static int _walk_aniHas = Animator.StringToHash("Walk");
    public static int _move_aniHas = Animator.StringToHash("Move");
    public static int _idel_aniHas = Animator.StringToHash("Idel");
    public static int _Jump_aniHas= Animator.StringToHash("Jump");
    public static int _Crawl_aniHas = Animator.StringToHash("Crawl");
    public static int _IdleWalkRun_aniHas = Animator.StringToHash("Speed");


    //--
    public const float EnemyIdleSpeed = 0f;
    public const float EnemyWalkSpeed = 1f;
    public const float EnemyRunSpeed = 2.5f;
    public const float PlayerMoveSpeed=2f;
    public const float PlayerWalkSpeed = 1f;

}
