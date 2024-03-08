/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月01日 星期三 11:11:33
* @Modify:
*/
using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace KamenGameFramewrok
{
    [Serializable]
    public class LocalResource
    {
        [FormerlySerializedAs("RootType")] [SerializeField]
        public AppRootType rootType;
        
        [FormerlySerializedAs("ResourceObj")] [SerializeField]
        public GameObject resourceObj;
    }
}