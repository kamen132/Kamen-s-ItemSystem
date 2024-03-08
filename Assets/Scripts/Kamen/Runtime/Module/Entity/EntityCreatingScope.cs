using System;
namespace KamenGameFramewrok
{
    public readonly struct EntityCreatingScope<T> : IDisposable where T: IEntity
    {
        public T Entity { get; }
        internal EntityCreatingScope(T entity)
        {
            Entity = entity;
        }
        public void Dispose()
        {
            KamenGame.Instance.EntityManager.EntityInitialized.Invoke(Entity);
        }
    }
}