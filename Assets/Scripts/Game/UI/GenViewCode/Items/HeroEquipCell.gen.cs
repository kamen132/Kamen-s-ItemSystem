﻿// 自动生成代码，请勿手动修改

using KamenGameFramewrok;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


#pragma warning disable 219
#pragma warning disable 414
namespace Game.UI.Code
{
    public partial class HeroEquipCell : UIItem
    {
        public override string AssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/ItemSystem/HeroEquipCell.prefab";
        public override string Name {get;protected set;} = @"HeroEquipCell";
        public string PrefabAssetPath {get;protected set;} = @"Assets/Resources_moved/Game/UI/ItemSystem/HeroEquipCell.prefab";

#region Control defines 

#endregion Control defines


#region Custom Control defines 


#endregion Custom Control defines 


		public HeroEquipCell() { }

        /// <summary>
        /// direct initialize root
        /// </summary>
        public HeroEquipCell(GameObject root)
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
