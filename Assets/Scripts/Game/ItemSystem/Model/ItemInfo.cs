namespace Game.ItemSystem
{
    /// <summary>
    /// 道具基类
    /// </summary>
    public class ItemInfo : ItemPresenterInfo
    {
        public string Uuid { get; private set; }
        public ItemInfo()
        {
            Category = EnumItemPresenterCategory.Item;
        }
    }
}