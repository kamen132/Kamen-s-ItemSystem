namespace Game.UI.Code
{
    public partial class UITestActionBarView
    {
        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            AddButton(_Button, (a, b) => { Close(); });
        }
    }
}