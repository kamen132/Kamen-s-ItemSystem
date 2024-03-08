/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 19:10:41
* @Modify:
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 资源加载模块
    /// </summary>
    public abstract class AssetModule : Module, IAssetModule
    {
        protected GameObject GameObject;
        protected readonly Dictionary<string, bool> ObjectHasPool = new Dictionary<string, bool>();
        protected readonly List<IAsset> Assets = new List<IAsset>();
        
        
        private readonly Queue<InstanceRequest> mInstanceOnePerFrameQueue = new Queue<InstanceRequest>();
        private readonly Queue<InstanceRequest> mInstanceSlowlyQueue = new Queue<InstanceRequest>();
        private readonly Queue<InstanceRequest> mInstanceQueue = new Queue<InstanceRequest>();
        private readonly Queue<LoadedCallback> mLoadAssetQueue = new Queue<LoadedCallback>();

        private readonly float UnloadUnusedAssetTime = 30.0f;
        protected float UnloadUnusedAssetTimer;

        public override void Update()
        {
            while (mInstanceQueue.Count > 0)
            {
                var request = mInstanceQueue.Dequeue();
                if (request.Callback != null)
                {
                    InstantiateAsync(request.AssetName, request.Callback);
                }
            }

            int nCount = 0;
            while (mInstanceSlowlyQueue.Count > 0 && nCount != 20)
            {
                var request = mInstanceSlowlyQueue.Dequeue();
                if (request.Callback != null)
                {
                    InstantiateAsync(request.AssetName, request.Callback);
                }

                nCount++;
            }

            if (mInstanceOnePerFrameQueue.Count > 0)
            {
                var request = mInstanceOnePerFrameQueue.Dequeue();
                if (request.Callback != null)
                {
                    InstantiateAsync(request.AssetName, request.Callback);
                }
            }

            while (mLoadAssetQueue.Count > 0)
            {
                var respond = mLoadAssetQueue.Dequeue();
                respond.Callback?.Invoke(respond.Asset);
            }

            UnloadUnusedAssetTimer += Time.deltaTime;
            if (UnloadUnusedAssetTimer >= UnloadUnusedAssetTime)
            {
                try
                {
                    UnloadUnusedAssets();
                }
                catch (Exception ex)
                {
                    KLogger.LogError(ex);
                }

                UnloadUnusedAssetTimer = 0f;
            }
        }

        public byte[] LoadFile(string path, bool readHotfix = true)
        {
            return File.ReadAllBytes(path);
        }

        public void LoadFileAsync(string path, Action<byte[]> completed)
        {
            UnityWebRequest webFile = UnityWebRequest.Get(path);
            webFile.SendWebRequest().completed += (AsyncOperation op) =>
            {
                completed?.Invoke(webFile.downloadHandler.data);
            };
        }

        public void InstantiateOnePerFrame(string assetName, Action<GameObject> completed)
        {
            var request = new InstanceRequest {AssetName = assetName, Callback = completed};
            mInstanceOnePerFrameQueue.Enqueue(request);
        }

        public void InstantiateSlowly(string assetName, Action<GameObject> completed)
        {
            var request = new InstanceRequest {AssetName = assetName, Callback = completed};
            mInstanceSlowlyQueue.Enqueue(request);
        }

        public void InstantiateNextFrame(string assetName, Action<GameObject> completed)
        {
            var request = new InstanceRequest {AssetName = assetName, Callback = completed};
            mInstanceQueue.Enqueue(request);
        }

        protected void LoadCallbackInUpdate(Action<IAsset> completed, IAsset asset)
        {
            var respond = new LoadedCallback {Asset = asset, Callback = completed};
            mLoadAssetQueue.Enqueue(respond);
        }

        public abstract IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed) where T : UnityEngine.Object;

        public IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed, UnityEngine.Object autoDestroyOwner) where T : UnityEngine.Object
        {
            IAsset asset = LoadAssetAsync<T>(assetName, completed);
            asset.Attack(autoDestroyOwner);
            Assets.Add(asset);
            return asset;
        }

        public abstract IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed);
        public abstract void InstantiateAsync(string assetName, Action<GameObject> completed);

        public virtual void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        public virtual void Destroy(GameObject gameObject, float fadeTime)
        {
            Object.Destroy(gameObject, fadeTime);
        }

        public virtual void Instantiate(string path, Action<GameObject> completed)
        {
            
        }

        public virtual void UnloadUnusedAssets()
        {
        }

        public GameObject Instantiate(GameObject gameObject)
        {
            return Object.Instantiate(gameObject);
        }
    }

    public struct InstanceRequest
    {
        public Action<GameObject> Callback;
        public string AssetName;
    }

    public struct LoadedCallback
    {
        public Action<IAsset> Callback;
        public IAsset Asset;
    }
}