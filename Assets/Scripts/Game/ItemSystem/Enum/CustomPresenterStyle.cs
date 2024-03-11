using Game.UI.Code;

namespace Game.ItemSystem
{
   public class CustomPresenterStyle
    {
        public enum EnumStyleType
        {
            PresenterStyle,
            HeroSmallCardStyle,
            ItemCellStyle,
            HeroEquipStyle
        }

        public EnumStyleType Type { get; private set; } = EnumStyleType.PresenterStyle;
        private EnumPresenterStyle mPresenterStyle;
        
        public EnumPresenterStyle PresenterStyle
        {
            get { return mPresenterStyle; }
            set
            {
                mPresenterStyle = value;
                Type = EnumStyleType.PresenterStyle;
            }
        }
        
        
        public CustomPresenterStyle(EnumPresenterStyle style)
        {
            PresenterStyle = style;
        }
        
        private EnumHeroEquipStyle mHeroEquipCardStyle;

        public EnumHeroEquipStyle HeroEquipCardStyle {
            get { return mHeroEquipCardStyle; }
            set
            {
                mHeroEquipCardStyle = value;
                Type = EnumStyleType.HeroEquipStyle;
            }
        }

        private EnumItemCellStyle mItemCellStyle;
        public EnumItemCellStyle ItemCellStyle {
            get { return mItemCellStyle; }
            set
            {
                mItemCellStyle = value;
                Type = EnumStyleType.ItemCellStyle;
            }
        }  
        
        private   EnumHeroCardCellStyle mHeroSmallCardStyle;
        public EnumHeroCardCellStyle HeroSmallCardStyle {
            get { return mHeroSmallCardStyle; }
            set
            {
                mHeroSmallCardStyle = value;
                Type = EnumStyleType.HeroSmallCardStyle;
            }
        }  
    }
}