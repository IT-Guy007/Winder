using DataModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace Winder;

public partial class ChatView {
    private DatabaseChangeListener databaseChangeListener;

    //MAUI
    private readonly ScrollView scrollView;
    private readonly StackLayout verticalStackLayout;
    private readonly Grid grid;

    public ChatView(User sendFromUser, User sendToUser) {

        Database.SetRead(sendFromUser.email, sendToUser.email);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });

        //MAUI
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
        
        
        Title = "Chat with your match now!";
        
        //Initialise content
        databaseChangeListener = new DatabaseChangeListener();
        databaseChangeListener.Initialize(sendFromUser, sendToUser);

        //All the chatmessages
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
                Border chatBorder = new Border {
                    Padding = 10,
                    Margin = 10,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                };

                if (message.fromUser == Authentication._currentUser.email) {
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
                Database.SendMessage(sendFromUser.email, sendToUser.email, chatInput.Text);
                Navigation.PushAsync(new ChatView(sendFromUser, sendToUser));
            }
        };

        inputStackLayout.Add(chatInput);
        inputStackLayout.Add(sendButton);
        
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

        //Set content
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");

        grid.SetRow(backButton, 0);
        grid.Add(backButton);
        grid.SetRow(scrollView, 1);
        grid.Add(scrollView);
        grid.SetRow(inputStackLayout, 2);
        grid.Add(inputStackLayout);
        
        scrollView.Content = verticalStackLayout;
        Content = grid;
        
    }
    
}