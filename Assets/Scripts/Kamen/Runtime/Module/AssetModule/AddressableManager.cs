using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace KamenGameFramewrok
{
    public class AddressableManager : MonoSingleton<AddressableManager>
    {
        private List<object> _updateKeys = new List<object>();

        private void Start()
        {
            UpdateCatalog();
        }

        public async void UpdateCatalog()
        {
            KLogger.Log("初始化Addressable");
            var init = Addressables.InitializeAsync(); //开始连接服务器初始化，检测是否连接服务器成功!
            await init.Task;

            var handle = Addressables.CheckForCatalogUpdates(false);
            await handle.Task;
            KLogger.Log("check catalog status " + handle.Status);
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> catalogs = handle.Result;
                if (catalogs != null && catalogs.Count > 0)
                {
                    KLogger.Log("download catalog start ");
                    var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                    await updateHandle.Task;

                    foreach (var item in updateHandle.Result)
                    {
                        _updateKeys.AddRange(item.Keys);
                    }

                    KLogger.Log("download catalog finish " + updateHandle.Status);
                    DownLoad();
                }
                else
                {
                    KLogger.Log("dont need update catalogs");
                }
            }

            Addressables.Release(handle);
        }

        public IEnumerator DownAssetImpl()
        {
            var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
            yield return downloadsize;
            KLogger.Log("start download size :" + downloadsize.Result);

            if (downloadsize.Result > 0)
            {
                var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
                yield return download;
                foreach (var item in (List<IAssetBundleResource>) download.Result)
                {
                    var ab = item.GetAssetBundle();
                    foreach (var name in ab.GetAllAssetNames())
                    {
                        KLogger.Log("asset name " + name);
                    }
                }

                Addressables.Release(download);
            }

            Addressables.Release(downloadsize);
        }

        public void DownLoad()
        {
            StartCoroutine(DownAssetImpl());
        }

        public async void DownloadAsset(object assetName, Action action)
        {
            var downloadsize = Addressables.GetDownloadSizeAsync(assetName);
            await downloadsize.Task;
            KLogger.Log("start download size :" + downloadsize.Result);

            if (downloadsize.Result > 0)
            {
                var download = Addressables.DownloadDependenciesAsync(assetName);
                await download.Task;
                foreach (var item in (List<IAssetBundleResource>) download.Result)
                {
                    var ab = item.GetAssetBundle();
                    foreach (var name in ab.GetAllAssetNames())
                    {
                        KLogger.Log("asset name " + name);
                    }
                }

                Addressables.Release(download);
            }

            Addressables.Release(downloadsize);

            action();
        }
    }
}
