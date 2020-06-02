using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEnum;
using LevelData;

public class ZombiesMananger : MonoBehaviour
{
    private WaveManger WaveManger;
    private Dictionary<int, List<EnemyController>> zombiesControllderDic = new Dictionary<int, List<EnemyController>>();
    List<ZombiesData> zombiesData;
    List<EnemyController> enemyControllers;
    Dictionary<int, List<Transform>> zombiesActiveDic;
    private WaveData _WaveData;
    public int _opneWave;
    private void Awake()
    {
        GameContorlManager._Instance._zombiesMananger = this;
        WaveManger = this.transform.GetComponent<WaveManger>();
        zombiesActiveDic = WaveManger.zombiesActiveDic;
        _WaveData = MyGame.WaveData.Find(f => f.WaveID == SaveManangr.Get.PlayerInfo.Level);
        _opneWave = 0;
    }

    public void Start()
    {
        LoadZombies();
    } 

    public void LoadZombies()
    {
        GameWaveData data = MyGame._GameWaveData;
        Data LevelData = data.Datas.Find(f => f.Level == SaveManangr.Get.m_playerInfo.Level-1);
        for (int i = 0; i < LevelData.LevelData.Count; i++)
        {
            zombiesControllderDic.Add(i, zombiesList(LevelData.LevelData[i].zombiesArray));
        }
        CorrectZombiesPositionAndDefaultState();
    }

    /// <summary>
    /// 矫正位置和默认状态 Info 基础数据
    /// </summary>
    public void CorrectZombiesPositionAndDefaultState()
    {
        for (int i = 0; i < zombiesControllderDic.Count; i++)
        {
            List<EnemyController> Enemys = zombiesControllderDic[i];
            List<Transform> transforms = zombiesActiveDic[i];
            for (int j = 0; j < Enemys.Count; j++)
            {
                EnemyController controllers = Enemys[j];
                int count = Mathf.CeilToInt(j % transforms.Count);
                Debug.LogError(transforms[count].position, transforms[count].gameObject);
                controllers.transform.position = transforms[count].position;
                controllers.transform.rotation = transforms[count].rotation;
                controllers.info();
                if (transforms[i].tag == Tag.CrawlingTriggle)
                    controllers.m_EnemyMobile.aiState = EnemyMobile.AIState.Crawl;
                else if (transforms[i].tag==Tag.JumpTrigger)
                    controllers.m_EnemyMobile.aiState = EnemyMobile.AIState.Jump;
                if ((_WaveData.Assault && i == zombiesActiveDic.Count - 1 || Random.Range(0, 1) == 1) && DBZombiesType.Batman == controllers.m_ZombiesData.ZombiesType)
                    controllers.m_EnemyMobile._defaultAiState = EnemyAniState.Move;
                else
                    controllers.m_EnemyMobile._defaultAiState = EnemyAniState.Walk;
            }
        }
        ActiveEnemy();
    }

    public List<EnemyController> zombiesList(List<int> enemyIDArray)
    {
        List<EnemyController> list = new List<EnemyController>();
        for (int i = 0; i < 1; i++)//enemyIDArray.Count
        {
            DBZombiesID zombiesid = DBZombiesID.TZ_Tank_03_Root;// (DBZombiesID)enemyIDArray[i];
            GameObject obj = Instantiate<GameObject>(Resource.Get.Zomboes.Find(f => f.ID == zombiesid).GameObject, Vector3.zero, Quaternion.identity);
            EnemyController enemyController = obj.GetComponent<EnemyController>(); enemyController._ID = enemyIDArray[i];
            list.Add(enemyController); obj.SetActive(false);
        }
        return list;
    }

    public void ActiveEnemy()
    {
        if (_opneWave < zombiesControllderDic.Count)
        {
            enemyControllers = zombiesControllderDic[_opneWave];
            _opneWave++;
            foreach (var item in enemyControllers)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
            DebugUtility.DebugLogError("游戏结束");
    }
    private void FixedUpdate()
    {
        if (enemyControllers.Count > 0)
            for (int i = 0; i < enemyControllers.Count; i++)
            {
                enemyControllers[i].m_Update();
            }
    }

    public void RemoveZombiesController(EnemyController enemy)
    {
        if (enemyControllers.Count > 0)
        {
            if (enemyControllers.Contains(enemy))
            {
                enemyControllers.Remove(enemy);
                if (enemyControllers.Count == 0)
                    ActiveEnemy();
            }
        }
    }
}
