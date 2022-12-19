using DataModel;

namespace Winder;

public partial class ChatView : ContentPage {
    
    public string originPage;
    
    
    //MAUI
    private ScrollView scrollView;
    
    //Chatmodel

    public ChatView(User sendFromUser, User sendToUser) {
        DatabaseChangeListener.Initialize(sendFromUser, sendToUser);
        this.originPage = "ChatPage";

        //MAUI
        scrollView = new ScrollView();
        Title = "Chat with your match now!";


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
        }
        
        
        
        
        //Set content
        Content = scrollView;
    }
    
}