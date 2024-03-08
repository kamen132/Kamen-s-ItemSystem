using KamenGameFramewrok;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KamenGameFramewrok
{
    internal class ObjectPool
    {
        private readonly Stack<GameObject> m_pool = new Stack<GameObject>();

        private string mPoolObjName = string.Empty;

        private int mSpawnNumber;

        private int mMAXSpawnNumber = ObjectPoolMgr.m_default_max_spawn_number;

        private float mLastDeSpawnTime = 3.40282347E+38f;

        public float lastDeSpawnTime
        {
            get
            {
                return mLastDeSpawnTime;
            }
            set
            {
                mLastDeSpawnTime = value;
            }
        }

        public int avaliableCount => m_pool.Count;

        public void Spawn(string resourceName, Action<GameObject> action)
        {
            if (mPoolObjName == string.Empty)
            {
                mPoolObjName = resourceName;
            }
            Spawn(action);
        }

        private void Spawn(Action<GameObject> action)
        {
            lastDeSpawnTime = 3.40282347E+38f;
            if (m_pool.Count > 0)
            {
                GameObject gameObject = m_pool.Pop();
                gameObject.transform.SetParent(null);
                gameObject.SetActive(true);
                gameObject.GetComponent<ObjectPoolItem>().OnObjectSpawn();
                action?.Invoke(gameObject);
                return;
            }
            //> 先不管上限
            //if (m_max_spawn_number >= 0 && m_spawn_number >= m_max_spawn_number)
            //{
            //    action?.Invoke(null);
            //    return;
            //}
            //if(m_PoolObjName == "")
            //{
            //    return;
            //}
            KamenGame.Instance.AssetModule.LoadAssetAsync<GameObject>(mPoolObjName, (IAsset asset) =>
            {
                GameObject gameObject2 = KamenGame.Instance.AssetModule.Instantiate(asset.Assets() as GameObject);
                asset.Attack(gameObject2);
                ObjectPoolItem component = gameObject2.GetComponent<ObjectPoolItem>();
                if (component != null)
                {
                    component.poolName = mPoolObjName;
                    component.OnObjectSpawn();
                    mMAXSpawnNumber = component.maxSpawnNumber;
                    mSpawnNumber++;
                }
                else
                {
                    Debug.LogError($"{mPoolObjName} not ObjectPoolItem");
                }
                action?.Invoke(gameObject2);
            }, null);
        }

        public void DeSpawn(GameObject obj, string ResourceName)
        {
            lastDeSpawnTime = Time.realtimeSinceStartup;
            if (m_pool.Contains(obj))
            {
                KLogger.LogError("ObjectPoolMgr : despawn object already in stack = " + obj.name);
                return;
            }
            if (mPoolObjName != ResourceName)
            {
                return;
            }
            obj.transform.SetParent(ObjectPoolMgr.GetInstance().m_handler.transform);
            obj.GetComponent<ObjectPoolItem>().OnObjectDeSpawn();
            obj.SetActive(false);
            m_pool.Push(obj);
        }

        public void Clear()
        {
            while (m_pool.Count != 0)
            {
                UnityEngine.Object.Destroy(m_pool.Pop());
            }
            m_pool.Clear();
            mSpawnNumber = 0;
        }

        public void TryClean()
        {
            if (Time.realtimeSinceStartup - lastDeSpawnTime >= ObjectPoolMgr.GetInstance().m_clean_pool_time)
            {
                Clear();
            }
        }
    }
}
