namespace GreenCareApp;
using GreenCareApp.View;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new LoginPage());
	}
}
