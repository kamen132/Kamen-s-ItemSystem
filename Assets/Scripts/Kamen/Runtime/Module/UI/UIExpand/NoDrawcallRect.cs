/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:14
* @Modify:
*/
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 优化组件，无Drawcall矩形区域，可以替代透明的Image使用
    /// </summary>
    public class NoDrawcallRect : Graphic
    {
        public NoDrawcallRect()
        {
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }

}