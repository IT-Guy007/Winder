using DataModel;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace Winder;

public partial class ChatPage {

    //MAUI
    private ScrollView scrollView;
    private StackLayout verticalStackLayout;
    private Grid grid;

    private readonly User sendFromUser;
    private readonly User sendToUser;

    public ChatPage(User sendFromUser, User sendToUser) {
        this.sendFromUser = sendFromUser;
        this.sendToUser = sendToUser;

        Database.SetRead(sendFromUser.Email, sendToUser.Email);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        
        //Set content
        Initialize();
        
    }

    private void Initialize() {
        //MAUI
        Title = "Chat with your match now!";
        scrollView = new ScrollView { 
            Orientation = ScrollOrientation.Vertical, 
            VerticalOptions = LayoutOptions.Fill,

        };

        verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical, 
            VerticalOptions = LayoutOptions.Fill
        };

        grid = new Grid {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#CC415F"),

            ColumnDefinitions = {
                new ColumnDefinition()
            },
            
            RowDefinitions = {
                new RowDefinition { Height = new GridLength(50)},
                new RowDefinition(),
                new RowDefinition { Height = new GridLength(50)}
            }
        };
        
        
        //Initialise content
        Authentication.GetChatMessages(sendFromUser, sendToUser);

        //First row
        HorizontalStackLayout horizontalStackLayout = new HorizontalStackLayout {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#CC415F"),
        };


        ImageButton backButton = new ImageButton {
            Source = "backbutton.png",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            WidthRequest = 50,
            HeightRequest = 50
        };
        backButton.Clicked += (_,_) => {
            Navigation.PushAsync(new ChatsViewPage());
        };
        
        horizontalStackLayout.Add(backButton);
        
        ImageButton refreshButton = new ImageButton {
            Source = "refresh.png",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            WidthRequest = 50,
            HeightRequest = 50
        };
        refreshButton.Clicked += (_,_) => {
            Initialize();
        };
        horizontalStackLayout.Add(refreshButton);

        //All the chatmessages
        if (Authentication.ChatCollection.Count == 0) {
            Label noMessagesFound = new Label {
                Text = "No messages found", 
                HorizontalOptions = LayoutOptions.Center, 
                VerticalOptions = LayoutOptions.Center,
                FontSize = 20
            };
            verticalStackLayout.Add(noMessagesFound);
        } else  {
            foreach (var message in Authentication.ChatCollection) {
                Border chatBorder = new Border {
                    Padding = 10,
                    Margin = 10,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                };

                if (message.fromUser == Authentication.CurrentUser.Email) {
                    //From me
                    chatBorder.HorizontalOptions = LayoutOptions.End;
                    chatBorder.StrokeShape = new BoxView() {
                        CornerRadius = new CornerRadius(10, 10, 10, 0)
                    };
                    chatBorder.BackgroundColor = Color.FromArgb("#ffffff");
                    chatBorder.Stroke = message.read ? Color.FromArgb("#2B0B98") : Color.FromArgb("#808080");
                    chatBorder.StrokeThickness = 5;
                    chatBorder.Content = new Label {
                        Text = message.message,
                        TextColor = Colors.Black,
                        FontSize = 20
                    };

                } else {
                    //From other
                    chatBorder.HorizontalOptions = LayoutOptions.Start;
                    chatBorder.StrokeShape = new RoundRectangle {
                        CornerRadius = new CornerRadius(0, 10, 10, 10)
                    };
                    chatBorder.BackgroundColor = Color.FromArgb("#25D366");
                    chatBorder.Content = new Label {
                        Text = message.message,
                        TextColor = Colors.White,
                        FontSize = 20
                    };
                }

                verticalStackLayout.Add(chatBorder);
            }
        }
        
        //TextInput
        StackLayout inputStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand};
        Entry chatInput = new Entry {
            Placeholder = "Type your message here",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            FontSize = 20,
            BackgroundColor = Colors.White,
            TextColor = Colors.Black
        };
        
        Button sendButton = new Button {
            Text = "Send",
            HorizontalOptions = LayoutOptions.End,
            FontSize = 20
        };

        sendButton.Clicked += (_, _) => {
            if (!string.IsNullOrWhiteSpace(chatInput.Text)) {
                chatInput.Text = char.ToUpper(chatInput.Text[0]) + chatInput.Text.Substring(1);
                Database.SendMessage(sendFromUser.Email, sendToUser.Email, chatInput.Text);
                Initialize();
            }
        };

        inputStackLayout.Add(chatInput);
        inputStackLayout.Add(sendButton);

        //Set content
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");

        grid.SetRow(horizontalStackLayout, 0);
        grid.Add(horizontalStackLayout);
        grid.SetRow(scrollView, 1);
        grid.Add(scrollView);
        grid.SetRow(inputStackLayout, 2);
        grid.Add(inputStackLayout);
        
        scrollView.Content = verticalStackLayout;
        Content = grid;
        
    }
    
}