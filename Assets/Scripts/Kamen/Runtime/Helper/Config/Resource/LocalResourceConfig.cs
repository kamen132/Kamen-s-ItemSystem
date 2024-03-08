/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月01日 星期三 11:11:43
* @Modify:
*/
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace KamenGameFramewrok
{
    [CreateAssetMenu(menuName="ScriptObject/LocalResource")]
    [Serializable]
    public class LocalResourceConfig : ScriptableObject
    {
        [FormerlySerializedAs("LocalResources")] [SerializeField]
        public List<LocalResource> localResources = new List<LocalResource>();
        
        public GameObject GetAppRoot(AppRootType rootType)
        {
            GameObject targetRoot = new GameObject(rootType+"(Default)");
            foreach (var data in localResources)
            {
                if (data.rootType==rootType)
                {
                    targetRoot = Object.Instantiate(data.resourceObj);
                }
            }
            return targetRoot;
        }
    }
}