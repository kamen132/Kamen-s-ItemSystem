namespace Game.UI.Code
{
    public partial class UITestActionBarViewB
    {
        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            AddButton(_Button, (a, b) => { Close(); });
        }
    }
}