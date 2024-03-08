/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:37
* @Modify:
*/

using UnityEngine;

namespace KamenGameFramewrok
{
    public class KamenApp : GameApp
    {
        protected override void OnAddPlugin()
        {
            base.OnAddPlugin();
            PluginManager.Registered(new CorePlugin());
        }
    }
}