using DataModel;
using Intents;
using Microsoft.Maui.Controls.Shapes;

namespace Winder;

public partial class ChatView {
    private DatabaseChangeListener databaseChangeListener;

    //MAUI
    private ScrollView scrollView;
    private StackLayout mainStackLayout;
    private StackLayout verticalStackLayout;
    private Grid grid;


    public ChatView(User sendFromUser, User sendToUser) {
        
        //MAUI
        scrollView = new ScrollView();
        mainStackLayout = new StackLayout{Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill};
        verticalStackLayout = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Fill };

        grid = new Grid {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#CC415F"),

            ColumnDefinitions = {
                new ColumnDefinition()
            },
            
            RowDefinitions = {
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
            VerticalOptions = LayoutOptions.End,
            FontSize = 20
        };
        
        Button sendButton = new Button {
            Text = "Send",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            FontSize = 20
        };
        
        inputStackLayout.Add(chatInput);
        inputStackLayout.Add(sendButton);

        
        //Set content
        
        mainStackLayout.Add(verticalStackLayout);
        mainStackLayout.Add(inputStackLayout);
        scrollView.Content = mainStackLayout;
        verticalStackLayout.BackgroundColor = Color.FromArgb("#CC415F");
        scrollView.BackgroundColor = Color.FromArgb("#CC415F");

        grid.SetRow(verticalStackLayout, 0);
        grid.Add(verticalStackLayout);
        grid.SetRow(inputStackLayout, 1);
        grid.Add(inputStackLayout);
        
        
        Content = grid;
        ;
    }
    
}