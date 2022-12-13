using Microsoft.Maui.Dispatching;
using System.Windows.Threading;

namespace Winder;

public partial class LaunchView : ContentPage
{
    private readonly DispatcherTimer _timer;
    public LaunchView()
	{
		InitializeComponent();
	}
}