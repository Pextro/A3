//using CloudKit;
using Microsoft.Maui.Controls;
using GreenCareApp.entities;
using SQLite;
using System.Text.RegularExpressions;

namespace GreenCareApp.View;

public partial class CadastroUsuarioPage : ContentPage {
    static DataBase database;

    public static DataBase Database {
        get {
            if (database == null) {
                database = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3"));
            }
            return database;
        }
    }

    public CadastroUsuarioPage() {
        InitializeComponent();

        MainPage mp = new MainPage();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        collectionView.ItemsSource = await CadastroUsuarioPage.Database.GetPessoaAsync();
    }

    async void OnCadastroClicked(object sender, EventArgs e) {
        if (!string.IsNullOrWhiteSpace(nomeUsuario.Text) && !string.IsNullOrWhiteSpace(emailUsuario.Text) && !string.IsNullOrWhiteSpace(senhaUsuario.Text)) {
            if (!IsValidEmail(emailUsuario.Text)) {
                await DisplayAlert("Atenção!", "Digite um email válido", "Fechar");
                return;
            }

            if (senhaUsuario.Text.Length < 6) {
                await DisplayAlert("Atenção!", "A senha deve ter pelo menos 6 caracteres", "Fechar");
                return;
            }


            // Verificar se o e-mail já está cadastrado
            using (SQLiteConnection conexao = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3"))) {
                List<Pessoa> emailPessoa = conexao.Query<Pessoa>("SELECT email FROM Pessoa where email = ?", emailUsuario.Text);


                if (emailPessoa.Any()) {
                    await DisplayAlert("Atenção!", "Este e-mail já está cadastrado", "Fechar");
                    return;
                }
            }

            var cadastro = await CadastroUsuarioPage.Database.SavePessoaAsync(new Pessoa {
                nome = nomeUsuario.Text,
                email = emailUsuario.Text,
                senha = senhaUsuario.Text
            });

            if (cadastro > 0) {
                await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "Fechar");
            }

            nomeUsuario.Text = emailUsuario.Text = senhaUsuario.Text = string.Empty;
            collectionView.ItemsSource = await CadastroUsuarioPage.Database.GetPessoaAsync();
        } else {
            await DisplayAlert("Atenção!", "Preencha todas as informações", "Fechar");
            return;
        }
        /*if (!string.IsNullOrWhiteSpace(nomeUsuario.Text) && !string.IsNullOrWhiteSpace(emailUsuario.Text) && !string.IsNullOrWhiteSpace(senhaUsuario.Text))
        {
            await CadastroUsuarioPage.Database.SavePessoaAsync(new Pessoa
            {
                nome = nomeUsuario.Text,
                email = emailUsuario.Text,
                senha = senhaUsuario.Text
            });

            nomeUsuario.Text = emailUsuario.Text = senhaUsuario.Text = string.Empty;
            collectionView.ItemsSource = await CadastroUsuarioPage.Database.GetPessoaAsync();
        } else {
            await DisplayAlert("ERRO", "Preencha todos os campos!", "Okey");
        }*/
    }

    private bool IsValidEmail(string email) {
        // Expressão regular para validar um endereço de email simples.
        string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    private async void focusEntry(object sender, EventArgs e) {
        if (sender is Entry entrada) {
            if (entrada is Element elemento) {
                await Task.Delay(300);
                await scroll.ScrollToAsync(elemento, ScrollToPosition.Start, true);
            } else {
                await DisplayAlert("ERRO", "Entrada is not element", "Okey");
            }
        } else {
            await DisplayAlert("ERRO", "Sender is not entry", "Okey");
        }
    }
}