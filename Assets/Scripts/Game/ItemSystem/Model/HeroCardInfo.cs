namespace Game.ItemSystem
{
    public class HeroCardInfo : ItemPresenterInfo
    {
        public string Uuid { get; private set; }
        public string Level { get; private set; }
        public string Exp { get; private set; }
        //.....
        public HeroCardInfo()
        {
            Category = EnumItemPresenterCategory.Hero;
        }
    }
}