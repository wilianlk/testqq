using CommunityToolkit.Maui.Views;

namespace Exportacion;

public partial class PhotoPopupPage : Popup
{
    public PhotoPopupPage(ImageSource imageSource)
    {
        InitializeComponent();
        PopupImage.Source = imageSource;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.Close();
    }
}
