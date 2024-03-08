// 自动生成代码，请勿手动修改

using KamenGameFramewrok;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


#pragma warning disable 219
#pragma warning disable 414
namespace Game.UI.Code
{
    public partial class UITestActionBarViewA : UIActionBarBaseView
    {
        public override string AssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/UITestActionBarViewA.prefab";
        public override string Name {get;protected set;} = @"UITestActionBarViewA";
        public string PrefabAssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/UITestActionBarViewA.prefab";

#region Control defines 
		/// <summary>
		/// Image
		/// Button
		/// </summary>
		protected GameObject _Button;
		protected Button mButtonBtn;
		protected Image mButtonImg;


#endregion Control defines

#region Custom Control defines



#endregion Custom Control defines

        protected override void OnInitializeAsset()
        {
            base.OnInitializeAsset();
            {
                InstantiateAsset(AssetPath);

                if( null == this.Root )
                {
                    return;
                }
            }
        }
        
        protected override void FindComponent()
        {    
#region Control assigners 
			_Button = this.Root.transform.Find("Button")?.gameObject;
			mButtonBtn = _Button.GetComponent<Button>();
			mButtonImg = _Button.GetComponent<Image>();


#endregion Control assigners 

#region Custom Control assigners 


#endregion Custom Control assigners

#region Control event registers 

#endregion Control event registers
                 
        }

#region Control event processors 

#endregion Control event processors 
    }
}
#pragma warning restore 219
#pragma warning restore 414
