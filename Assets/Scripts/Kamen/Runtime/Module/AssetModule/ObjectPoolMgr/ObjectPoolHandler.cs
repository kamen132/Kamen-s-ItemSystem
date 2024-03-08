using System;
using UnityEngine;

namespace KamenGameFramewrok
{
    internal class ObjectPoolHandler : MonoBehaviour
    {
        private void Start()
        {
            ObjectPoolMgr.GetInstance().m_handler = base.transform.gameObject;
        }

        private void OnDestroy()
        {
            ObjectPoolMgr.GetInstance().ClearAllPool();
        }
    }
}