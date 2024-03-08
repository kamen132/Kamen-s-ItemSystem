using System;
using System.Collections.Generic;

namespace KamenGameFramewrok
{
     public class EntityManager: Module, IEntityManager
    {
        public List<IEntity> Entities { get; } = new List<IEntity>();
        private readonly Dictionary<EntityType, List<IEntity>> mFactionEntities = new Dictionary<EntityType, List<IEntity>>();
        private readonly Dictionary<int, Entity> mId2EntityMapping = new Dictionary<int, Entity>();
        private readonly List<int> mEntityToBeRemoved = new List<int>();
        private int mNextEntityIndex = 0;
        private Action<IEntity> mEntityInitialized;
        public Action<IEntity> EntityInitialized => mEntityInitialized;
        public override void Init()
        {
            base.Init();
            mEntityInitialized = OnEntityInitialized;
        }

        public override void Update()
        {
            CommitRemove();
            foreach (var entity in Entities)
            {
                entity.Update();
            }
        }

        public override void FixedUpdate()
        {
            foreach (var entity in Entities)
            {
                entity.FixedUpdate();
            }
        }

        private void OnEntityInitialized(IEntity eventdata)
        {
            if (!mFactionEntities.TryGetValue(eventdata.Type, out var list))
            {
                list = new List<IEntity>(32);
                mFactionEntities[eventdata.Type] = list;
            }

            list.Add(eventdata);
        }

        public void Dispose()
        {
            Reset();
        }

        internal void CommitRemove()
        {
            mEntityToBeRemoved.Clear();
            if (Entities.Count <= 0)
            {
                return;
            }

            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                var entity = Entities[i];
                if (!entity.IsAlive)
                {
                    mEntityToBeRemoved.Add(entity.Index);
                    Entities.RemoveAt(i);
                }
            }

            if (mEntityToBeRemoved.Count > 0)
            {
                foreach (var keyValuePair in mFactionEntities)
                {
                    for (int i = keyValuePair.Value.Count - 1; i >= 0; i--)
                    {
                        var entity = keyValuePair.Value[i];
                        if (!entity.IsAlive)
                        {
                            keyValuePair.Value.RemoveAt(i);
                        }
                    }
                }
            }

            foreach (var entityId in mEntityToBeRemoved)
            {
                if (this.mId2EntityMapping.TryGetValue(entityId, out var entity))
                {
                    entity.Dispose();
                }

                this.mId2EntityMapping.Remove(entityId);
            }

            mEntityToBeRemoved.Clear();
        }

        public void Reset()
        {
            foreach (var iterator in Entities)
            {
                iterator.Dispose();
            }

            this.Entities.Clear();
            this.mId2EntityMapping.Clear();
            this.mEntityToBeRemoved.Clear();
            mNextEntityIndex = 0;
        }

        public bool TryGetByIndex(int entityIndex, out IEntity entity)
        {
            if (this.mId2EntityMapping.TryGetValue(entityIndex, out var entityInstance))
            {
                entity = entityInstance;
                return true;
            }

            entity = default;
            return false;
        }
        public T CreateEntity<T>() where T : Entity, new()
        {
            using (var entityCreatingScope = Create<T>())
            {
                return entityCreatingScope.Entity;
            }
        }

        public EntityCreatingScope<T> Create<T>() where T : Entity,new()
        {
            T entity = new T();
            entity.Init(mNextEntityIndex++);
            Entities.Add(entity);
            mId2EntityMapping[entity.Index] = entity;
            return new EntityCreatingScope<T>(entity);
        }

        public void Remove(IEntity entity)
        {
            int entityIndex = entity.Index;
            if (this.mId2EntityMapping.TryGetValue(entityIndex, out var entityInstance))
            {
                entityInstance.MarkAsPendingRemove();
            }
        }

        public void Remove(int entityIndex)
        {
            if (this.mId2EntityMapping.TryGetValue(entityIndex, out var entityInstance))
            {
                entityInstance.MarkAsPendingRemove();
            }
        }

        public void ForeachEntity(Action<IEntity> action)
        {
            foreach (var entity in Entities)
            {
                if (entity.IsAlive)
                {
                    action(entity);
                }
            }
        }
    }
}