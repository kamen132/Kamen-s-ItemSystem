using System;
using System.Collections.Generic;

namespace KamenGameFramewrok
{
    public class Entity : IEntity
    {
        private enum EState : byte
        {
            Alive,
            PendingRemoved,
            Disposed,
        }
        private EState mState = EState.Alive;
        public EntityType Type { get; protected set; }
        public int Index { get; private set; }
        public bool IsAlive => mState == EState.Alive;
        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        private Dictionary<string, IComponent> mComponents;

        internal Entity()
        {
            mComponents = new Dictionary<string, IComponent>();
        }

        public void Init(int index)
        {
            Index = index;
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
        

        internal void MarkAsPendingRemove()
        {
            if (this.mState != EState.Disposed)
            {
                this.mState = EState.PendingRemoved;    
            }
        }
        
        public virtual void Dispose()
        {
            if (mState == EState.Disposed)
            {
                return;
            }
            this.mState = EState.Disposed;
        }
        
        public KComponent GetComponent<KComponent>() where KComponent : class, IComponent
        {
            Type typeOfComponent = typeof(KComponent);
            mComponents.TryGetValue(typeOfComponent.ToString(),out IComponent component);
            return component as KComponent;
        }

        public KComponent AddComponent<KComponent>() where KComponent : class, IComponent
        {
            Type typeOfComponent = typeof(KComponent);
            var component = (IComponent)Activator.CreateInstance(typeOfComponent,this);
            if (!mComponents.TryGetValue(typeOfComponent.ToString(),out IComponent aa))
            {
                mComponents.Add(component.ToString(),component);
            }
            return component as KComponent;
        }
        public void RemoveComponent<KComponent>() where KComponent : class, IComponent
        {
            Type typeOfComponent = typeof(KComponent);
            var component = (IComponent)Activator.CreateInstance(typeOfComponent,this);
            mComponents.Remove(component.ToString());
        }

        public void ClearComponent()
        {
            foreach (var component in mComponents)
            {
                mComponents.Remove(component.Key);
            }
        }
        
        public override string ToString()
        {
            return $"单位({this.Index})";
        }
    }
}