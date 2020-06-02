
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using ClassEnum;


namespace Pool
{
	public class PoolManager : MonoBehaviour
	{

		private static PoolManager _instance = null;

		public static PoolManager Get
		{
			get
			{
				if (_instance==null)
				{
					_instance = new GameObject("PoolManager").AddComponent<PoolManager>();
				}
				return _instance;
			}
		}

		/// <summary> 
		/// 刷新清理时间 负数不清理
		/// </summary>
		public float RefreshTime = 15;
		private bool _isCoroutines = false;
		private Transform _poolTransform;
		private Dictionary<PoolPrefabType, ObjectPool<GameObject>> _objectIdDict;
		//目标对象  可以直接放在节点下 自动获得
		public Dictionary<PoolPrefabType , GameObject> _objectPools; 

		private void Awake()
		{
			_instance = this;
			_objectIdDict = new Dictionary<PoolPrefabType, ObjectPool<GameObject>>();
			_objectPools = new Dictionary<PoolPrefabType, GameObject>();
			_poolTransform = this.transform;
			LoadPrefab();
			DontDestroyOnLoad(this);
		}

		private void LoadPrefab()
        {
            foreach (Transform item in this.transform.transform)
            {
				PoolPrefabType type;
				System.Enum.TryParse<PoolPrefabType>(item.name, out type);
				_objectPools.Add(type, item.gameObject);
			}
        }

		/// <summary>
		/// 拿到目标对象
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		public GameObject GetObj(PoolPrefabType prefab)
		{
			if (!_objectIdDict.ContainsKey(prefab))
			{
				GetPool(prefab);
			}

			GameObject obj = GetPool(prefab).Get();
			return obj;
		}

		public GameObject GetObj(PoolPrefabType prefab, Transform root, Vector3 position, Quaternion rotation)
		{
			var obj = GetObj(prefab);		
			obj.transform.position = position;
			obj.transform.rotation = rotation;
            if(root!=null)
		    obj.transform.parent = root;
            else
		    obj.transform.parent = this.transform;
			obj.gameObject.SetActive(true);
			return obj;
		}

		/// <summary>
		/// 返回物体对应的池子
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		private ObjectPool<GameObject> GetPool(PoolPrefabType prefab)
		{
			if (!_objectIdDict.ContainsKey(prefab))
			{
				var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); });
				pool.PoolName = prefab.ToString();
				_objectIdDict.Add(prefab, pool);
			}
			return _objectIdDict[prefab];
		}

		/// <summary>
		/// 释放目标对象 重新存储目标
		/// </summary>
		/// <param name="prefab"></param>
		/// <exception cref="Exception"></exception>
		public void Release(PoolPrefabType prefab,GameObject @object)
		{
			if (!_objectIdDict.ContainsKey(prefab))
			{
				throw new Exception("不存在" + prefab + "相关对象池");
			}
			@object.SetActive(false);
			var value = _objectIdDict[prefab];
			value.Release(@object);
		}


		/// <summary>
		/// 获取目标对象实例
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		private GameObject InstantiatePrefab(PoolPrefabType prefab)
		{
			GameObject go=null;
			if (!_objectPools[prefab])
				Debug.LogError("未包含资源===> " + prefab);
			else
            {
			 go = Instantiate(_objectPools[prefab]);
			 go.transform.SetParent(this.transform);
			 go.gameObject.SetActive(false);
			}
			return go;
		}

		/// <summary>
		/// 定时清理池子
		/// </summary>
		/// <returns></returns>
		IEnumerator ClearObjectPool()
		{
			while (true)
			{
				if (_objectPools.Count != 0)
				{
					foreach (var pair in _objectIdDict.ToList())
					{
						if (pair.Value.UserDict.Count == 0 && pair.Value.UnUseList.Count > 0)
						{
							Debug.Log("ObjectPool:" + pair.Key + "不活跃删除该池子");
                            for (int i = pair.Value.UnUseList.Count-1; i >=0; i--)
                            {
								Destroy(pair.Value.UnUseList[i].Item);
                            }
							 ObjectPool<GameObject> pool = pair.Value;
							_objectPools.Remove(pair.Key);
							pool = null;
						}
						if (_objectPools.Count == 0)
						{
							_isCoroutines = false;
							StopCoroutine("ClearObjectPool");
							Debug.Log("ObjectPool:清空池子，停止刷新");
						}
					}
				}
				yield return new WaitForSeconds(RefreshTime);
			}
		}

	}
}
