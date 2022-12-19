using DataModel;

namespace Winder;

public partial class ChatView : ContentPage {
    
    public string originPage;
    
    
    //MAUI
    private ScrollView scrollView;
    private StackLayout verticalStackLayout;
    
    //Chatmodel

    public ChatView(User sendFromUser, User sendToUser) {
        
        //MAUI
        scrollView = new ScrollView();
        verticalStackLayout = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };
        scrollView.Content = verticalStackLayout;
        Title = "Chat with your match now!";
        
        //Initialise content
        DatabaseChangeListener.Initialize(sendFromUser, sendToUser);
        this.originPage = "ChatPage";



        if(DatabaseChangeListener._chatCollection.Count == 0)
        foreach (var message in DatabaseChangeListener._chatCollection) {
            Label chatMessage = new Label();
            chatMessage.Text = message.message;
            chatMessage.FontSize = 20;
            if (message.fromUser == Authentication._currentUser.email) {
                chatMessage.HorizontalTextAlignment = TextAlignment.End;
                chatMessage.BackgroundColor = Color.FromArgb("#90EE90");
            } else {
                chatMessage.HorizontalTextAlignment = TextAlignment.Start;
                chatMessage.BackgroundColor = Color.FromArgb("#ADD8E6");
            }
            verticalStackLayout.Add(chatMessage);
        }
        
        
        
        
        //Set content
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");
        Content = scrollView;
    }
    
}