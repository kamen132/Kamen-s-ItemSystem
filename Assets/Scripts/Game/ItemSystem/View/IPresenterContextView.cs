namespace Game.ItemSystem.View
{
    public interface IPresenterContextView
    {
        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="ctx"></param>
        void SetContext(ItemPresenterInfo ctx);
        
        /// <summary>
        /// 点击
        /// </summary>
        void OnDefaultClickedHandler();
        
        /// <summary>
        /// 长按
        /// </summary>
        void OnDefaultLongPressedHandler();

        /// <summary>
        /// 置灰
        /// </summary>
        /// <param name="active"></param>
        void SetGray(bool active);
        
        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="isSelected"></param>
        void OnSelected(bool isSelected);

    }
}