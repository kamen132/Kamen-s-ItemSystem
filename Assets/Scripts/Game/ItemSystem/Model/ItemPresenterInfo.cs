﻿using System;
using UniRx;
 using Unity.VisualScripting;

 namespace Game.ItemSystem
{
    /// <summary>
    /// 物品基类
    /// </summary>
    public abstract class ItemPresenterInfo : IDisposable
    {
        /// <summary>
        /// 物品id
        /// </summary>
        public int Id { get; protected set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; protected set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Desc { get; protected set; }
        
        /// <summary>
        /// Icon
        /// </summary>
        public virtual string Icon { get; protected set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public ReactiveProperty<long> Count = new ReactiveProperty<long>();
        
        /// <summary>
        /// 种类
        /// </summary>
        public EnumItemPresenterCategory Category { get; protected set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public RewardType RewardType { get; protected set; }
        
        /// <summary>
        /// 品质
        /// </summary>
        public virtual ItemQualityColor Color { get; protected set; }
        
        
        /// <summary>
        /// 深拷贝一份数据
        /// </summary>
        /// <param name="newInfo"></param>
        public virtual void Clone(ItemPresenterInfo newInfo)
        {
            
        }

        public ItemPresenterInfo()
        {
            
        }

        public ItemPresenterInfo(ItemPresenterInfo newInfo)
        {
            Clone(newInfo);
        }
        
        public virtual void ParseDataFormServer()
        {
            
        }
        
        public virtual void ParseDataConfig()
        {
            
        }
        
        public void Dispose()
        {
            Count = null;
        }
    }
}