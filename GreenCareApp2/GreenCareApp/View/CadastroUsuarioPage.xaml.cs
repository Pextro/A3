//using CloudKit;
using Microsoft.Maui.Controls;
using GreenCareApp.entities;


namespace GreenCareApp.View;

public partial class CadastroUsuarioPage : ContentPage
{
    static DataBase database;

    public static DataBase Database
    {
        get
        {
            if (database == null)
            {
                database = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3"));
            }
            return database;
        }
    }

    public CadastroUsuarioPage()
    {
        InitializeComponent();

        MainPage mp = new MainPage();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        collectionView.ItemsSource = await CadastroUsuarioPage.Database.GetPessoaAsync();
    }

    async void OnCadastroClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(nomeUsuario.Text) && !string.IsNullOrWhiteSpace(emailUsuario.Text) && !string.IsNullOrWhiteSpace(senhaUsuario.Text))
        {
            await CadastroUsuarioPage.Database.SavePessoaAsync(new Pessoa
            {
                nome = nomeUsuario.Text,
                email = emailUsuario.Text,
                senha = senhaUsuario.Text
            });

            nomeUsuario.Text = emailUsuario.Text = senhaUsuario.Text = string.Empty;
            collectionView.ItemsSource = await CadastroUsuarioPage.Database.GetPessoaAsync();
        }
    }
    private async void focusEntry(object sender, EventArgs e)
    {
        if (sender is Entry entrada)
        {
            if (entrada is Element elemento)
            {
                await Task.Delay(300);
                await scroll.ScrollToAsync(elemento, ScrollToPosition.Start, true);
            }
            else
            {
                await DisplayAlert("ERRO", "Entrada is not element", "Okey");
            }
        }
        else
        {
            await DisplayAlert("ERRO", "Sender is not entry", "Okey");
        }
    }
}