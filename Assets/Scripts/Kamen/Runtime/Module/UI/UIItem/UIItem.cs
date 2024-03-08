/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月28日 星期六 11:10:53
* @Modify:
*/
namespace KamenGameFramewrok
{
    public class UIItem : UIContainer
    {
        /// <summary>
        /// UI容器
        /// </summary>
        public UIContainer UIContainer { get; private set; }
        public void AttachToContainer(UIContainer container)
        {
            UIContainer = container;
        }
        
        /// <summary>
        /// 从UI容器剥离出来
        /// </summary>
        public void DetachFromContainer()
        {
            if (UIContainer != null)
            {
                UIContainer = null;
            }
        }
        
        public virtual void OnReset()
        {
            
        }
    }
}
