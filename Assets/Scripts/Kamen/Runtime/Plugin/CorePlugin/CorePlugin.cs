
namespace KamenGameFramewrok
{
    public class CorePlugin : Plugin
    {
        public CorePlugin() : base("基础功能")
        {
        }

        public override void AddModule()
        {
            AddModule<IUIManager>(new UIManager());
            AddModule<IAssetModule>(new AddressAssetModule());
            AddModule<IEntityManager>(new EntityManager());
            AddModule<IInputManager>(new InputManager());
        }
    }
}