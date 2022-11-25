using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace MAUI;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnProfilePictureClicked(object sender, EventArgs e)
    {
        var image = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Kies een profielfoto",
            FileTypes = FilePickerFileType.Images
        });

        if (image == null)
        {
            return;
        }

        var stream = await image.OpenReadAsync();
        ProfileImage.Source = ImageSource.FromStream(() => stream);
    }

}