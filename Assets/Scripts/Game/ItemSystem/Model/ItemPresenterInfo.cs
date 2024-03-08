﻿using System;
using UniRx;

namespace Game.ItemSystem
{
    public class ItemPresenterInfo : IDisposable
    {
        public int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Desc { get; protected set; }
        public virtual string Icon { get; protected set; }
        public ReactiveProperty<long> Count = new ReactiveProperty<long>();
        public EnumItemPresenterCategory Category { get; protected set; }
        public RewardType RewardType { get; protected set; }
        public virtual ItemQualityColor Color { get; protected set; }

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