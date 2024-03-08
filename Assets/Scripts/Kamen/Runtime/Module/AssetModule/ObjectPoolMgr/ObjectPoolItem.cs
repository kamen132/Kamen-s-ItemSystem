using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KamenGameFramewrok
{
    public class ObjectPoolItem : MonoBehaviour
    {
        public int m_max_spawn_number_high = 3;

        public int m_max_spawn_number_mid = 2;

        public int m_max_spawn_number_low = 1;

        private int m_max_spawn_number = ObjectPoolMgr.m_default_max_spawn_number;

        private List<ObjectPoolItem> m_childPoolItem;

        private string m_pool_name;

        private Vector3 m_org_scale = Vector3.one;

        public int maxSpawnNumber
        {
            get { return m_max_spawn_number; }
            set { m_max_spawn_number = value; }
        }

        public string poolName
        {
            get { return m_pool_name; }
            set { m_pool_name = value; }
        }

        private void UpdateMaxSpawnNumber()
        {
            if (KamenGame.Instance.GetGraphicLevel() == GraphicLevel.HIGH)
            {
                maxSpawnNumber = m_max_spawn_number_high;
            }
            else if (KamenGame.Instance.GetGraphicLevel() == GraphicLevel.MEDIUM)
            {
                maxSpawnNumber = m_max_spawn_number_mid;
            }
            else
            {
                maxSpawnNumber = m_max_spawn_number_low;
            }
        }

        private void Awake()
        {
            m_org_scale = transform.lossyScale;
            UpdateMaxSpawnNumber();
        }

        public void Disable(float delay)
        {
            StartCoroutine(_disable(delay));
        }

        private IEnumerator _disable(float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                Disable();
            }
            catch (Exception e)
            {
                KLogger.LogError(e);
            }
        }

        public virtual void Reset()
        {
        }

        public bool IsActive()
        {
            return gameObject.activeInHierarchy;
        }

        public virtual void Disable()
        {
            ObjectPoolMgr.GetInstance().GetPool(poolName).DeSpawn(gameObject, poolName);
        }

        public virtual void OnObjectSpawn()
        {
            SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        }

        public virtual void OnObjectDeSpawn()
        {
            PreDeSpawnChild();
            transform.localScale = m_org_scale;
            SendMessage("OnDeSpawn", SendMessageOptions.DontRequireReceiver);
        }

        public void SearchParent()
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                ObjectPoolItem componentInParent = parent.GetComponentInParent<ObjectPoolItem>();
                if (componentInParent != null)
                {
                    componentInParent.RegisterChild(this);
                }
            }
        }

        public void RegisterChild(ObjectPoolItem obj)
        {
            if (m_childPoolItem == null)
            {
                m_childPoolItem = new List<ObjectPoolItem>();
            }

            m_childPoolItem.Add(obj);
        }

        private void PreDeSpawnChild()
        {
            if (m_childPoolItem != null)
            {
                for (int i = 0; i < m_childPoolItem.Count; i++)
                {
                    if (m_childPoolItem[i] != null)
                    {
                        m_childPoolItem[i].Disable();
                    }
                }

                m_childPoolItem.Clear();
            }
        }
    }
}