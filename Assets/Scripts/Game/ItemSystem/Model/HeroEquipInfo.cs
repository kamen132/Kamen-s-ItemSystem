namespace Game.ItemSystem
{
    /// <summary>
    /// 英雄装备
    /// </summary>
    public class HeroEquipInfo : ItemPresenterInfo
    {
        public string Uuid { get; private set; }
        public string OwnHeroUuid { get; private set; }
        public string Level { get; private set; }
        //.....
        public HeroEquipInfo()
        {
            Category = EnumItemPresenterCategory.HeroEquip;
        }
    }
}