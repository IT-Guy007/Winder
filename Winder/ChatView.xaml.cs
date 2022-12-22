using DataModel;

namespace Winder;

public partial class ChatView : ContentPage {
    private DatabaseChangeListener databaseChangeListener;
    
    public string originPage;

    //MAUI
    private ScrollView scrollView;
    private StackLayout mainStackLayout;
    private StackLayout verticalStackLayout;

    public ChatView(User sendFromUser, User sendToUser) {

        Database.makeRead(sendFromUser.email, sendToUser.email);

        //MAUI
        scrollView = new ScrollView();
        mainStackLayout = new StackLayout{Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill};
        verticalStackLayout = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };
        Title = "Chat with your match now!";
        
        //Initialise content
        databaseChangeListener = new DatabaseChangeListener();
        databaseChangeListener.Initialize(sendFromUser, sendToUser);
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

        sendButton.Clicked += (_, _) => {
            if (!string.IsNullOrWhiteSpace(chatInput.Text)) {
                chatInput.Text = char.ToUpper(chatInput.Text[0]) + chatInput.Text.Substring(1);
                Database.SendMessage(sendFromUser.email, sendToUser.email, chatInput.Text);
                Navigation.PushAsync(new ChatView(sendFromUser, sendToUser));
            }
        };

        inputStackLayout.Add(chatInput);
        inputStackLayout.Add(sendButton);

        //Set content
        
        mainStackLayout.Add(verticalStackLayout);
        mainStackLayout.Add(inputStackLayout);
        scrollView.Content = mainStackLayout;
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");
        Content = scrollView;
    }
    
}