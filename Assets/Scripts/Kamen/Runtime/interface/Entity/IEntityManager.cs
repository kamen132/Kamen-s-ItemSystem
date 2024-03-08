using System;
using System.Collections.Generic;

namespace KamenGameFramewrok
{
    public interface IEntityManager : IDisposable ,IModule
    {
        bool TryGetByIndex(int entityIndex, out IEntity entity);

        T CreateEntity<T>() where T : Entity, new();

        EntityCreatingScope<T> Create<T>() where T : Entity, new();

        void Remove(IEntity entity);
        
        void Remove(int entityIndex);

        void ForeachEntity(Action<IEntity> action);
        
        List<IEntity> Entities { get; }
        Action<IEntity> EntityInitialized { get; }
    }
}