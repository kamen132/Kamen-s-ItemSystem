/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:03
* @Modify:
*/
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KamenGameFramewrok.Editor
{
    class ViewCodeTemplate
    {
        /// <summary>
        /// 位置标记
        /// </summary>
        public enum PlaceHolder : int
        {
            __FILE_HEADER__,
            __NAMESPACE_INCLUDE__,
            __NAMESPACE__,
            __CLASS_NAME__,
            __ASSET_PATH__,
            __PREFAB_PATH__,
            __CONTROL_DEFINES__,
            __CONTROL_ASSIGNERS__,
            __CUSTOM_CONTROL_DEFINES__,
            __CUSTOM_CONTROL_ASSIGNERS__,
            MAX,
        }

        // 模板内容
        private string mContent;

        // 导出内容
        private string mExportContent;

        // 内容替换对照字典
        private Dictionary<PlaceHolder, string> mContent2Replace = new Dictionary<PlaceHolder, string>();


        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="path">模板路径</param>
        public void Load(string path)
        {
            this.mContent = File.ReadAllText(path);
        }

        /// <summary>
        /// 模板已加载
        /// </summary>
        public bool IsLoaded
        {
            get { return null != this.mContent; }
        }


        public void ReplaceBegin()
        {
            mContent2Replace.Clear();
            // 初始化一遍,解决所有存在模板中的PlaceHolder都应该被替换掉
            int maxCount = (int) PlaceHolder.MAX;
            for (int i = 0; i < maxCount; i++)
            {
                mContent2Replace.Add((PlaceHolder) i, string.Empty);
            }
        }

        public void Replace(PlaceHolder placeHolder, string content)
        {
            Append(placeHolder, content);
        }

        public void Append(PlaceHolder placeHolder, string content)
        {
            if (mContent2Replace.ContainsKey(placeHolder))
            {
                mContent2Replace[placeHolder] += content;
            }
            else
            {
                mContent2Replace.Add(placeHolder, content);
            }
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="path">导出路径</param>
        public void Export(string path)
        {
            mExportContent = mContent;
            string strDir = Path.GetDirectoryName(path);
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }

            foreach (var iter in mContent2Replace)
            {
                mExportContent = mExportContent.Replace(iter.Key.ToString(), iter.Value);
            }

            File.WriteAllText(path, this.mExportContent, Encoding.UTF8);
        }

    }
}