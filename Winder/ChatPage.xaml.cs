using DataModel;
using System.Drawing;
using static System.Drawing.Graphics;
namespace MAUI;

public partial class ChatPage : ContentPage {
    private Database _database = new Database();
    private User FromUser;
    public User ToUser;

    public ChatPage(User fromUser, User toUser) {
        FromUser = fromUser;
        ToUser = toUser;

        Title = "Make your match now!";

        //Main stacklayout
        StackLayout verticalStackLayout = new StackLayout
            { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };
        verticalStackLayout.Spacing = 10;
        

    }
}