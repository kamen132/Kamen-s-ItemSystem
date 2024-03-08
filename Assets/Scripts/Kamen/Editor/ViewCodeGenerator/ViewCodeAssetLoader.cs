/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:41
* @Modify:
*/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KamenGameFramewrok.Editor
{
    class ViewCodeAssetLoader : IDisposable
    {
        public GameObject Target { get; private set; }

        /// <summary>
        /// 内存镜像名
        /// </summary>
        public string AssetBundleName { get; private set; }

        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetName { get; private set; }

        public string PrefabAssetPath { get; private set; }

        private Dictionary<string, List<GameObject>> mNamedGameObjects = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// 名字与对象对照字典
        /// </summary>
        public Dictionary<string, List<GameObject>> NamedGameObjects
        {
            get { return mNamedGameObjects; }
        }

        // 搜索到的要导出的节点
        private List<GameObject> mExportedControls;

        public void Load(string assetBundleName, GameObject target, string prefabPath)
        {
            Target = target;
            AssetName = target.name;
            AssetBundleName = assetBundleName;
            PrefabAssetPath = prefabPath;
        }

        /// <summary>
        /// 已加载
        /// </summary>
        public bool IsLoaded
        {
            get { return null != this.Target; }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void Unload()
        {
            if (Target != null)
            {
                Target = null;
            }
        }

        /// <summary>
        /// 搜索器
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="goContinue">是否继续搜索</param>
        public delegate bool Searcher<in T>(T obj, out bool goContinue);

        public static List<GameObject> AdvancedSearchChildren(GameObject thiz, Searcher<GameObject> searcher)
        {
            if (thiz == null || searcher == null)
            {
                return null;
            }

            List<GameObject> result = new List<GameObject>();

            Predicate<GameObject> queryer = (go) =>
            {
                bool goContinue;
                if (searcher(go, out goContinue))
                {
                    result.Add(go);
                }

                return goContinue;
            };

            AdvancedChildrenForeach(thiz, queryer);

            return result;
        }

        /// <summary>
        /// 子节点遍历
        /// </summary>
        /// <param name="thiz">当前对象</param>
        /// <param name="action">执行回调</param>
        /// <param name="recursively">递归开关</param>
        public static void AdvancedChildrenForeach(GameObject thiz, Predicate<GameObject> action)
        {
            if (thiz == null || action == null)
            {
                return;
            }

            for (int i = 0; i < thiz.transform.childCount; ++i)
            {
                GameObject child = thiz.transform.GetChild(i).gameObject;
                bool recursively = action.Invoke(child);
                if (recursively)
                {
                    AdvancedChildrenForeach(child, action);
                }
            }
        }

        /// <summary>
        /// 子节点遍历
        /// </summary>
        /// <param name="thiz">当前对象</param>
        /// <param name="action">执行回调</param>
        /// <param name="recursively">递归开关</param>
        public static void ChildrenForeach(GameObject thiz, Action<GameObject> action, bool recursively = true)
        {
            if (thiz == null || action == null)
            {
                return;
            }

            for (int i = 0; i < thiz.transform.childCount; ++i)
            {
                GameObject child = thiz.transform.GetChild(i).gameObject;
                action.Invoke(child);

                if (recursively)
                {
                    ChildrenForeach(child, action, true);
                }
            }
        }

        /// <summary>
        /// 获取从root子节点到当前对象之间的节点路径
        /// 例如节点结构"root/node/thiz",结果就是"node/thiz"
        /// </summary>
        /// <param name="thiz">当前对象</param>
        /// <param name="root">root节点</param>
        /// <param name="divider">分隔物</param>
        /// <returns>节点路径</returns>
        public static string GetPath(GameObject thiz, GameObject root, char divider = '/')
        {
            if (thiz.transform.parent == null || root == null)
            {
                return null;
            }

            StringBuilder ret = new StringBuilder(thiz.name);

            GameObject curObj = thiz;
            curObj = curObj.transform.parent.gameObject;

            while (curObj != root && curObj.transform.parent != null)
            {
                ret.Insert(0, divider);
                ret.Insert(0, curObj.name);
                curObj = curObj.transform.parent.gameObject;
            }

            return ret.ToString();
        }

        /// <summary>
        /// 搜索子节点
        /// </summary>
        /// <param name="thiz">当前对象</param>
        /// <param name="filter">条件过滤器</param>
        /// <returns>找到的子节点集合</returns>
        public static List<GameObject> SearchChildren(GameObject thiz, Predicate<GameObject> filter)
        {
            if (thiz == null || filter == null)
            {
                return null;
            }

            List<GameObject> result = new List<GameObject>();

            Action<GameObject> queryer = (go) =>
            {
                if (filter(go))
                {
                    result.Add(go);
                }
            };

            ChildrenForeach(thiz, queryer);

            return result;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="gameObjectWithExportTagOnly">是否仅导出被标记了的对象</param>
        public void Export(bool gameObjectWithExportTagOnly = true)
        {
            if (null == Target)
            {
                return;
            }

            {
                mExportedControls = AdvancedSearchChildren(this.Target, ExportSearcher);
            }

            UpdateNames();
        }

        /// <summary>
        /// 导出搜索器
        /// </summary>
        private bool ExportSearcher(GameObject go, out bool goContinue)
        {
            goContinue = true;

            if (!go.CompareTag("Export"))
            {
                if (go.CompareTag("UIView") || go.CompareTag("UIItem")) //如果发现对象是UIView/UIItem那么导出对象，并且不继续向下查找
                {
                    goContinue = false;
                    return true;
                }

                return false;
            }

            return true;
        }

        // 过滤对象名
        private string GETGameObjectName(GameObject go)
        {
            StringBuilder trimedRet = new StringBuilder();

            // 要过滤的字符
            HashSet<char> trimTokens = new HashSet<char>()
            {
                '(',
                ')',
                ' ',
            };

            string ret = string.Format("_{0}", go.name);
            foreach (char ch in ret)
            {
                if (trimTokens.Contains(ch))
                {
                    continue;
                }

                trimedRet.Append(ch);
            }

            return trimedRet.ToString();
        }

        private void UpdateNames()
        {
            this.NamedGameObjects.Clear();
            foreach (GameObject go in this.mExportedControls)
            {
                string goName = GETGameObjectName(go);
                List<GameObject> goWithSameName = null;

                if (!NamedGameObjects.TryGetValue(goName, out goWithSameName))
                {
                    goWithSameName = new List<GameObject>();
                    NamedGameObjects.Add(goName, goWithSameName);
                }

                goWithSameName.Add(go);
            }
        }

        public string GetGameObjectPath(GameObject go)
        {
            string path = GetPath(go, this.Target);
            return path;
        }

        public void Dispose()
        {
            this.Target = null;
        }
    }
}