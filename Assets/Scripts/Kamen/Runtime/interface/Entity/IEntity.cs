using System;

namespace KamenGameFramewrok
{
    public interface IEntity : IDisposable
    { 
        EntityType Type { get; }
        int Index { get;}
        bool IsAlive { get; }
        void Update();
        void FixedUpdate();
    }
}