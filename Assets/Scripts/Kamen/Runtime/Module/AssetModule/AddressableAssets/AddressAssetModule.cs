using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace KamenGameFramewrok
{
    public class AddressAssetModule : AssetModule
    {
        public override void Init()
        {
            KLogger.Log("start init Addressables" + Time.realtimeSinceStartup);
            Addressables.InitializeAsync(true).Completed += AddressAssetService_Completed;
        }

        public override IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                KLogger.LogError("LoadAssetAsync param assetName is null");
                return null;
            }

            var handler = Addressables.LoadAssetAsync<T>(assetName);
            if (handler.IsDone && handler.Result == null)
            {
                handler = Addressables.LoadAssetAsync<T>("ErrorPrefab");
            }

            var newAsset = new AddressAsset<T>(handler, assetName, UnloadUnusedAssetTimer);
            handler.Completed += (obj) =>
            {

                if (IsInitialized == false)
                {
                    return;
                }
                
                if (typeof(T) == typeof(GameObject))
                {
                    LoadCallbackInUpdate(completed, newAsset);
                }
                else
                {
                    completed?.Invoke(newAsset);
                }
            };
            return newAsset;
        }

        public override IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed)
        {
            var handler = Addressables.LoadSceneAsync(assetName, addictive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            var newAsset = new AddressSceneAsset(handler, assetName, UnloadUnusedAssetTimer);
            handler.Completed += (obj) => { completed?.Invoke(newAsset); };
            return newAsset;
        }

        public override void Instantiate(string assetName, Action<GameObject> completed)
        {
            if (ObjectHasPool.ContainsKey(assetName))
            {
                ObjectPoolMgr.GetInstance().GetPool(assetName).Spawn(assetName, completed);
                return;
            }

            var op = Addressables.LoadAssetAsync<GameObject>(assetName);
            GameObject go = op.WaitForCompletion();
            if (go == null)
            {
                KLogger.LogError($"GameObject is null AssetName:{assetName}");
                return;
            }

            ObjectPoolItem component = go.GetComponent<ObjectPoolItem>();
            if (component != null)
            {
                if (component.poolName == null || component.poolName.Equals(string.Empty))
                {
                    component.poolName = assetName;
                }

                ObjectPoolMgr.GetInstance().GetPool(component.poolName).Spawn(component.poolName, completed);
            }
            else
            {
                var @object = KamenGame.Instance.AssetModule.Instantiate(go);
                try
                {
                    completed?.Invoke(@object);
                }
                catch (Exception ex)
                {
                    KLogger.LogError($"{assetName} Callback Error: {ex.ToString()}");
                }
            }
        }

        public override void InstantiateAsync(string assetName, Action<GameObject> completed)
        {
            if (ObjectHasPool.ContainsKey(assetName))
            {
                ObjectPoolMgr.GetInstance().GetPool(assetName).Spawn(assetName, completed);
                return;
            }

            if (assetName.Equals(string.Empty))
            {
                KLogger.LogError("Instantiate assetName is null");
                return;
            }

            LoadAssetAsync<GameObject>(assetName, (IAsset asset) =>
            {
                GameObject gameObject;
                if (asset.Assets() == null)
                {
                    KLogger.LogError("no find " + assetName);
                    gameObject = new GameObject(assetName);
                    completed?.Invoke(gameObject);
                    return;
                }
                
                gameObject = asset.Assets() as GameObject;

                ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
                if (component != null)
                {
                    if (component.poolName == null || component.poolName.Equals(string.Empty))
                    {
                        component.poolName = assetName;
                    }

                    ObjectPoolMgr.GetInstance().GetPool(component.poolName).Spawn(component.poolName, completed);
                }
                else
                {
                    var @object = KamenGame.Instance.AssetModule.Instantiate(gameObject);
                    asset.Attack(@object);
                    try
                    {
                        completed?.Invoke(@object);
                    }
                    catch (Exception ex)
                    {
                        KLogger.LogError($"{assetName} Callback Error: {ex.ToString()}");
                    }
                }
            }, null);
        }

        public override void Destroy(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
            if (component != null && component.poolName != null)
            {
                KLogger.Log($"{gameObject.name},{gameObject.GetInstanceID()}");
                ObjectPoolMgr.GetInstance().GetPool(component.poolName).DeSpawn(gameObject, component.poolName);
                if (!ObjectHasPool.ContainsKey(component.poolName))
                {
                    ObjectHasPool.Add(component.poolName, true);
                }
            }
            else
            {
                Object.Destroy(gameObject);
            }
        }


        public override void UnloadUnusedAssets()
        {
            ObjectPoolMgr.GetInstance().TryCleanPool();

            for (int i = 0; i < Assets.Count; i++)
            {
                int status = Assets[i].Process();
                if (status == 0)
                {
                    break;
                }
                else if (Assets[i].Process() == 2)
                {
                    continue;
                }
                else if (status == 1)
                {
                    Assets.RemoveAt(i);
                    i--;
                }
            }

            UnloadUnusedAssetTimer = 0.0f;
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        private void AddressAssetService_Completed(AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> obj)
        {
            KLogger.Log("end init Addressable" + Time.realtimeSinceStartup);
            GameObject = new GameObject("AddressAssetModule");
            GameObject.AddComponent<ObjectPoolHandler>();
            Object.DontDestroyOnLoad(GameObject);
            ResourceManager.ExceptionHandler = null;
            OnInitialized();
        }
    }
}