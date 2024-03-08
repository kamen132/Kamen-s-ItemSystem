namespace Game.UI.Code
{
    public partial class UITestActionBarViewA
    {
        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            AddButton(_Button, (a, b) => { Close(); });
        }
    }
}