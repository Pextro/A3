using GreenCareApp.entities;

namespace GreenCareApp.View;


public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void btnAcessar(object sender, EventArgs e)
    {
        try {
            List<Pessoa> listaDePessoas = await CadastroUsuarioPage.Database.GetPessoaAsync();
            if (!string.IsNullOrWhiteSpace(emailUsuario.Text) && !string.IsNullOrWhiteSpace(senhaUsuario.Text)) {
                foreach (Pessoa pessoa in listaDePessoas) {
                    if (pessoa.email.Equals(emailUsuario.Text, StringComparison.OrdinalIgnoreCase) && pessoa.senha.Equals(senhaUsuario.Text)) {
                        await Navigation.PushAsync(new MenuPage(pessoa.Id));
                        return;
                    }

                }
                await DisplayAlert("ERRO", "Email ou senha incorreta!", "Okey");
            } else {
                await DisplayAlert("ERRO", "Preencha todos os campos!", "Okey");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    private async void btnCadastrarse(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroUsuarioPage());
    }
}