namespace GreenCareApp.View;


public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void btnAcessar(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new MenuPage());
    }

    private async void btnCadastrarse(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroUsuarioPage());
    }
}