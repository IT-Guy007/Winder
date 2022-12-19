using DataModel;

namespace MAUI;

public partial class Headertemplate : ContentPage
{
    Database database = new Database();
    public Headertemplate()
	{
		InitializeComponent();
	}

	
    
    

    private async void VideoButton_Clicked(object sender, EventArgs e)
    {
        var video = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Videos
        });

        if (video == null)
        {
            return;
        }
        else
        {

            byte[] videoinbytes = null;
            string email = "testest";
            

            var videopath = video.FullPath;
            videoinbytes = File.ReadAllBytes(videopath);
            database.SaveVideo(email, videoinbytes);
        }
        
    }

   
}