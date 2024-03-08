/*
* @Author: Kamen
* @Description:
* @Date: 2024年03月08日 星期五 16:03:13
* @Modify:
*/
namespace Game.ItemSystem
{
    /// <summary>
    /// 物品种类
    /// </summary>
    public enum EnumItemPresenterCategory
    {
        Item,
        Hero,
        HeroEquip,
        //....
    }
    
    /// <summary>
    /// 物品类型
    /// </summary>
    public enum RewardType
    {
        Gold,
        ExpCard,
        HeroMaterial,
        //....
    }

    public enum ItemQualityColor
    {
        Green,
        Blue,
        Gold,
        Red
    }
    
    public enum EnumPresenterStyle
    {
        NormalStyle,               //缺省状态
        EmptyStyle,                //空卡
        LockedStyle,               //锁定状态
        AddStyle,                  //添加状态
        //通用的状态：边框图+icon
        RewardIconOnlyStyle,       //奖励物品展示状态[资源类无边框，其他带边框]
        NoBorderIconOnlyStyle,     //只有icon图标
        BorderIconOnlyStyle,       //边框+图标
        BorderIconWithBgStyle,     //图标+背景
    }
}