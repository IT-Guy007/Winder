using DataModel;

namespace Winder;

public partial class ChatView : ContentPage {
    
    public string originPage;

    //MAUI
    private ScrollView scrollView;
    private StackLayout verticalStackLayout;


    public ChatView(User sendFromUser, User sendToUser) {
        
        //MAUI
        scrollView = new ScrollView();
        verticalStackLayout = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };
        scrollView.Content = verticalStackLayout;
        Title = "Chat with your match now!";
        
        //Initialise content
        DatabaseChangeListener.Initialize(sendFromUser, sendToUser);
        this.originPage = "ChatPage";



        if (DatabaseChangeListener._chatCollection.Count == 0) {
            Label noMessagesFound = new Label {
                Text = "No messages found", 
                HorizontalOptions = LayoutOptions.Center, 
                VerticalOptions = LayoutOptions.Center,
                FontSize = 20
            };
            verticalStackLayout.Add(noMessagesFound);
        } else  {

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
        }
        
        //TextInput
        StackLayout inputStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.End };
        Entry chatInput = new Entry {
            Placeholder = "Type your message here",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.End,
            FontSize = 20
        };
        
        Button sendButton = new Button {
            Text = "Send",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            FontSize = 20
        };
        sendButton.Clicked += async () => {
            if (chatInput.Text != "") {
                await DatabaseChangeListener.SendMessage(chatInput.Text);
                chatInput.Text = "";
            }
        };
        
        inputStackLayout.Add(chatInput);
        inputStackLayout.Add(sendButton);
        verticalStackLayout.Add(inputStackLayout);
        

        //Set content
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");
        Content = scrollView;
    }
    
}