using DataModel;

namespace Winder;

public partial class ChatView : ContentPage {
    
    public string originPage;
    
    //Users
    private User sendFromUser;
    private User sendToUser;
    
    //MAUI
    private ScrollView scrollView;
    
    //Chatmodel

    public ChatView(User sendFromUser, User sendToUser) {
        this.sendFromUser = sendFromUser;
        this.sendToUser = sendToUser;
        this.originPage = "ChatPage";

        //MAUI
        scrollView = new ScrollView();
        Title = "Chat with your match now!";
        
        
        
        
        
        
        //Set content
        Content = scrollView;
    }
    
}