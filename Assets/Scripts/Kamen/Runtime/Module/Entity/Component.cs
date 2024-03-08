namespace KamenGameFramewrok
{
    public class KComponent : IComponent
    {
        public IEntity Entity { get; private set; }

        public KComponent(IEntity entity)
        {
            Entity = entity;
        }
    }
}