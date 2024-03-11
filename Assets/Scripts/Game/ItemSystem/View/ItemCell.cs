using System;
using Game.ItemSystem;
using Game.ItemSystem.View;

namespace Game.UI.Code
{
    public enum EnumItemCellStyle
    {
        NormalStyle,
        BorderIconOnlyStyle,
        NoBorderIconOnlyStyle,
        BorderIconWithBgStyle,
    }
    public partial class ItemCell : IPresenterContextView
    {
        public void SetContext(ItemPresenterInfo ctx)
        {
            throw new System.NotImplementedException();
        }

        public void OnDefaultClickedHandler()
        {
            throw new System.NotImplementedException();
        }

        public void OnDefaultLongPressedHandler()
        {
            throw new System.NotImplementedException();
        }

        public void SetGray(bool active)
        {
            throw new System.NotImplementedException();
        }

        public void OnSelected(bool isSelected)
        {
            throw new System.NotImplementedException();
        }

        public void SetCardStyle(Enum @enum)
        {
            throw new NotImplementedException();
        }
    }
}