/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 18:10:32
* @Modify:
*/

using System;
using UnityEngine;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 资源加载模块接口
    /// </summary>
    public interface IAssetModule : IModule
    {
        /// <summary>
        /// 加载二进制
        /// </summary>
        /// <param name="path"></param>
        /// <param name="readHotfix"></param>
        /// <returns></returns>
        byte[] LoadFile(string path, bool readHotfix = true);
        
        /// <summary>
        /// 异步加载 - 二进制文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="completed"></param>
        void LoadFileAsync(string path, Action<byte[]> completed);
        
        /// <summary>
        /// 异步加载 - 泛型
        /// </summary>
        /// <param name="assetName">资源完整路径名</param>
        /// <param name="completed">加载完成回调</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed) where T:UnityEngine.Object;
        
        /// <summary>
        /// 异步加载 - 销毁
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="completed"></param>
        /// <param name="autoDestroyOwner"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed, UnityEngine.Object autoDestroyOwner) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="addictive"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed);
        
        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="completed"></param>
        void InstantiateAsync(string assetName, Action<GameObject> completed);
        
        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="assetName"></param        >
        /// <param name="completed"></param>
        void InstantiateNextFrame(string assetName, Action<GameObject> completed);
        
        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="completed"></param>
        void InstantiateOnePerFrame(string assetName, Action<GameObject> completed);
        
        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="completed"></param>
        void InstantiateSlowly(string assetName, Action<GameObject> completed);
        
        /// <summary>
        /// 销毁资源
        /// </summary>
        /// <param name="gameObject"></param>
        void Destroy(GameObject gameObject);
        
        /// <summary>
        /// 销魂资源 - 延迟销毁
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="fadeTime"></param>
        void Destroy(GameObject gameObject, float fadeTime);
        
        /// <summary>
        /// 实例化 - 传入GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        GameObject Instantiate(GameObject gameObject);

        void Instantiate(string path, Action<GameObject> completed);

        /// <summary>
        /// 卸载资源
        /// </summary>
        void UnloadUnusedAssets();
    }
}