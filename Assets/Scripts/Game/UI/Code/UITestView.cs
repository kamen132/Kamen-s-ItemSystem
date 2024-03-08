/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月01日 星期三 11:11:54
* @Modify:
*/
using KamenGameFramewrok;
using UnityEngine.UI;

namespace Game.UI.Code
{
    public partial class UITestView : UIView
    {
        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            AddButton(_Button, (a,b) => { Close(); });
            AddSubItemToNode<UITestItem>(_Button);
        }
    }
}