using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


namespace KamenGameFramewrok
{
    class AddressAsset<T> : IAsset where T : Object
    {
        private AsyncOperationHandle<T> mHanddler;
        private Queue<Object> mAttackObjects = new Queue<Object>();
        int mRefCount = 0;
        private string mAssetName;
        private float mReleaseTime = 0.0f;

        public AddressAsset(AsyncOperationHandle<T> handdler, string name, float fTime = 30.0f)
        {
            mHanddler = handdler;
            mAssetName = name;
            mReleaseTime = Time.realtimeSinceStartup + fTime;
        }

        public string AssetName()
        {
            return mAssetName;
        }

        public Object Assets()
        {
            return mHanddler.Result;
        }

        public Scene Scene()
        {
            return default(Scene);
        }

        public void UnLoadScene()
        {
        }

        public void Attack(Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
            if (mRefCount <= 0)
            {
                mRefCount = 0;
                KLogger.Log($"Release Handle:{mHanddler.Result.name}", Color.green);
                Addressables.Release(mHanddler);
                mAttackObjects.Clear();
            }
        }

        public void Retain()
        {
            mRefCount++;
        }


        public int Process()
        {
            if (Time.realtimeSinceStartup < mReleaseTime)
                return 0;

            if (mAttackObjects.Count > 0)
            {
                if (mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }

                if (mRefCount == 0)
                    return 1;

                return 2;
            }
            else
            {
                return 1;
            }
        }
    }


    class AddressSceneAsset : IAsset
    {
        private AsyncOperationHandle<SceneInstance> mHanddler;
        private Queue<Object> mAttackObjects = new Queue<Object>();
        int mRefCount = 0;
        private string mAssetName;
        private float mReleaseTime = 0.0f;

        public AddressSceneAsset(AsyncOperationHandle<SceneInstance> handdler, string name, float fTime)
        {
            mHanddler = handdler;
            mAssetName = name;
            mReleaseTime = Time.realtimeSinceStartup + fTime;
        }

        public string AssetName()
        {
            return mAssetName;
        }

        public Scene Scene()
        {
            return mHanddler.Result.Scene;
        }

        public void UnLoadScene()
        {
            Addressables.UnloadSceneAsync(mHanddler);
        }

        public Object Assets()
        {
            return null;
        }

        public void Attack(Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
            if (mRefCount <= 0)
            {
                mRefCount = 0;
                Addressables.Release(mHanddler);
                mAttackObjects.Clear();
            }
        }

        public void Retain()
        {
            mRefCount++;
        }

        public int RefCount()
        {
            return mRefCount;
        }

        public int Process()
        {
            if (Time.realtimeSinceStartup < mReleaseTime)
                return 0;

            // 目前先处理，只有绑定的才会处理自动销毁
            if (mAttackObjects.Count > 0)
            {
                if (mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }

                if (mRefCount == 0)
                    return 1;

                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}