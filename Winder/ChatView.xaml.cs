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
        
        
        
        
        
        
        //Set content
        Content = scrollView;
    }
    
}