namespace Game.ItemSystem
{
   public class CustomPresenterStyle
    {
        public enum EnumStyleType
        {
            PresenterStyle,
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
    }
}