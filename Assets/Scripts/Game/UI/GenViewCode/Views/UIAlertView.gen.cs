// 自动生成代码，请勿手动修改

using KamenGameFramewrok;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


#pragma warning disable 219
#pragma warning disable 414
namespace Game.UI.Code
{
    public partial class UIAlertView : UIView
    {
        public override string AssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/UIAlertView.prefab";
        public override string Name {get;protected set;} = @"UIAlertView";
        public string PrefabAssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/UIAlertView.prefab";

#region Control defines 
		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _BG;
		protected Image mBGImg;

		/// <summary>
		/// </summary>
		protected GameObject _Matter;


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
       			_BG = this.Root.transform.Find("BG")?.gameObject;
			mBGImg = _BG.GetComponent<Image>();

			_Matter = this.Root.transform.Find("Matter")?.gameObject;


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
