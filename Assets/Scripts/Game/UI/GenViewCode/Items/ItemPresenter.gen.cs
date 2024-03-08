// 自动生成代码，请勿手动修改

using KamenGameFramewrok;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


#pragma warning disable 219
#pragma warning disable 414
namespace Game.UI.Code
{
    public partial class ItemPresenter : UIItem
    {
        public override string AssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/ItemSystem/ItemPresenter.prefab";
        public override string Name {get;protected set;} = @"ItemPresenter";
        public string PrefabAssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/ItemSystem/ItemPresenter.prefab";

#region Control defines 
		/// <summary>
		/// </summary>
		protected GameObject _mPresenterStyle;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _m_buildBG;
		protected Image mBuildBGImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _mSprIconBG;
		protected Image mSprIconBGImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _m_sprLocked;
		protected Image mSprLockedImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _m_sprAdd;
		protected Image mSprAddImg;

		/// <summary>
		/// </summary>
		protected GameObject _mPresenterCard;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _mPresenterSelect;
		protected Image mPresenterSelectImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _GotImage;
		protected Image mGotImageImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _redDot;
		protected Image mRedDotImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _Lock;
		protected Image mLockImg;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _change;
		protected Image mChangeImg;

		/// <summary>
		/// Text
		/// </summary>
		protected GameObject _VirtualItemTxt;
		protected Text mVirtualItemTxtTxt;

		/// <summary>
		/// Image
		/// </summary>
		protected GameObject _BigLock;
		protected Image mBigLockImg;


#endregion Control defines


#region Custom Control defines 


#endregion Custom Control defines 


		public ItemPresenter() { }

        /// <summary>
        /// direct initialize root
        /// </summary>
        public ItemPresenter(GameObject root)
        {
            this.Root = root;
            this.Initialize();
        }

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
           			_mPresenterStyle = this.Root.transform.Find("mPresenterStyle")?.gameObject;

			_m_buildBG = this.Root.transform.Find("mPresenterStyle/m_buildBG")?.gameObject;
			mBuildBGImg = _m_buildBG.GetComponent<Image>();

			_mSprIconBG = this.Root.transform.Find("mPresenterStyle/mSprIconBG")?.gameObject;
			mSprIconBGImg = _mSprIconBG.GetComponent<Image>();

			_m_sprLocked = this.Root.transform.Find("mPresenterStyle/m_sprLocked")?.gameObject;
			mSprLockedImg = _m_sprLocked.GetComponent<Image>();

			_m_sprAdd = this.Root.transform.Find("mPresenterStyle/m_sprAdd")?.gameObject;
			mSprAddImg = _m_sprAdd.GetComponent<Image>();

			_mPresenterCard = this.Root.transform.Find("mPresenterCard")?.gameObject;

			_mPresenterSelect = this.Root.transform.Find("mPresenterSelect")?.gameObject;
			mPresenterSelectImg = _mPresenterSelect.GetComponent<Image>();

			_GotImage = this.Root.transform.Find("GotImage")?.gameObject;
			mGotImageImg = _GotImage.GetComponent<Image>();

			_redDot = this.Root.transform.Find("redDot")?.gameObject;
			mRedDotImg = _redDot.GetComponent<Image>();

			_Lock = this.Root.transform.Find("Lock")?.gameObject;
			mLockImg = _Lock.GetComponent<Image>();

			_change = this.Root.transform.Find("change")?.gameObject;
			mChangeImg = _change.GetComponent<Image>();

			_VirtualItemTxt = this.Root.transform.Find("VirtualItemTxt")?.gameObject;
			mVirtualItemTxtTxt = _VirtualItemTxt.GetComponent<Text>();

			_BigLock = this.Root.transform.Find("BigLock")?.gameObject;
			mBigLockImg = _BigLock.GetComponent<Image>();


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
