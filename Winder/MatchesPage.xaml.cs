using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Views;
using DataModel;
using MAUI;
using System.Collections.ObjectModel;

namespace Winder;

public partial class MatchesPage : ContentPage
{
	public string originPage;
    Database Database = new Database();
	public class MatchedPerson
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ImageSource ProfilePicture { get; set; }
        public MatchedPerson(User MatchedStudent)
        {
            FirstName = MatchedStudent.firstName;
            LastName = MatchedStudent.lastName;
            MemoryStream ms = new MemoryStream(MatchedStudent.profilePicture);
            ProfilePicture = ImageSource.FromStream(() => ms);
        }

    }
	public MatchesPage()
	{
		InitializeComponent();
        List<User> MatchedStudents = Database.GetMatchedStudentsFromUser(Authentication._currentUser.email);
		List<MatchedPerson> MatchedPeople = ConvertUserToMatchPerson(MatchedStudents);
		ListOfMatches.ItemsSource = MatchedPeople;
    }
    private List<MatchedPerson> ConvertUserToMatchPerson(List<User> MatchedStudents)
    {
		List<MatchedPerson> MatchedPeople = new List<MatchedPerson>();
        foreach (var student in MatchedStudents)
		{
			MatchedPeople.Add(new MatchedPerson(student));
		}
		return MatchedPeople;
    }

    private void Backbutton_Clicked(object sender, EventArgs e)
	{

		
		switch (originPage)
		{
			case "matchpage":
				Navigation.PushAsync(new MatchPage());
				break;
			case "profilepage":
				Navigation.PushAsync(new ProfileChange());
				break;
			
		}


	}
}