using Microsoft.Maui.Controls;
using GreenCareApp.entities;
using SQLite;

namespace GreenCareApp.View;

public partial class MenuPage : TabbedPage
{
    private int id;
    bool cadastroVisivel = false;
    bool jardimVisivel = false;
    bool cadastroItemVisivel = false;
    bool itemVisivel = false;


	public MenuPage(int id)
	{
        this.id = id;
		InitializeComponent();
	}

    private void cadastrarPlanta(object sender, EventArgs e) {
        if (!cadastroVisivel) {
            labelNomePlanta.IsVisible = labelEspecie.IsVisible = labelLocalizacao.IsVisible = nomePlanta.IsVisible = especiePlanta.IsVisible = localizacaoPlanta.IsVisible = botaoSalvarPlanta.IsVisible = true;
            cadastroVisivel = true;
        } else {
            labelNomePlanta.IsVisible = labelEspecie.IsVisible = labelLocalizacao.IsVisible = nomePlanta.IsVisible = especiePlanta.IsVisible = localizacaoPlanta.IsVisible = botaoSalvarPlanta.IsVisible = false;
            cadastroVisivel = false;
        }
    }

    private async void btnSalvarPlanta(object sender, EventArgs e) {
        if (!string.IsNullOrWhiteSpace(nomePlanta.Text) && !string.IsNullOrWhiteSpace(especiePlanta.Text) && !string.IsNullOrWhiteSpace(localizacaoPlanta.Text)) {
            await CadastroUsuarioPage.Database.SavePlantaAsync(new Planta {
                nome = nomePlanta.Text,
                especie = especiePlanta.Text,
                localizacao = localizacaoPlanta.Text,
                idPessoa = id
            });
            nomePlanta.Text = especiePlanta.Text = localizacaoPlanta.Text = string.Empty;
            await Navigation.PushAsync(new MenuPage(id));
        }
    }

    private void cadastrarItem(object sender, EventArgs e) {
        if (!cadastroItemVisivel) {
            labelNomeItem.IsVisible = nomeItem.IsVisible = labelQuantidade.IsVisible = quantidadeItem.IsVisible = botaoSalvarItem.IsVisible = true;
            cadastroItemVisivel = true;
        } else {
            labelNomeItem.IsVisible = nomeItem.IsVisible = labelQuantidade.IsVisible = quantidadeItem.IsVisible = botaoSalvarItem.IsVisible = false;
            cadastroItemVisivel = false;
        }
    }

    private async void btnSalvarItem(object sender, EventArgs e) {
        if (!string.IsNullOrWhiteSpace(nomeItem.Text) && !string.IsNullOrWhiteSpace(quantidadeItem.Text)) {
            await CadastroUsuarioPage.Database.SaveItemAsync(new Item {
                nome = nomeItem.Text,
                quantidade = int.Parse(quantidadeItem.Text),
                idPessoa = id
            });
            nomeItem.Text = quantidadeItem.Text = string.Empty;
            await Navigation.PushAsync(new MenuPage(id));
        }
    }

    private void mostrarJardim(object sender, EventArgs e) {
        if (!jardimVisivel) {
            collectionViewPlanta.IsVisible = true;
            jardimVisivel = true;
        } else {
            collectionViewPlanta.IsVisible = false;
            jardimVisivel = false;
        }
    }

    private async void btnExcluiPlanta(object sender, EventArgs e) {
        if (sender is Button button) {
            if (button.CommandParameter is CollectionView collectionView) {
                collectionView.SelectedItem = button.BindingContext;
                if (collectionView.SelectedItem is Planta selectedItem) {
                    int idPlanta = selectedItem.Id;
                    using (SQLiteConnection conexao = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3"))) {
                        conexao.Delete<Planta>(idPlanta);
                        /*string query = "DELETE FROM Planta WHERE Id = @Id";
                        SQLiteCommand comando = new SQLiteCommand(conexao);
                        comando.CommandText = query;
                        comando.CommandText
                        comando.ExecuteNonQuery();*/
                    }
                    await Navigation.PushAsync(new MenuPage(id));
                } else {
                    await DisplayAlert("ERRO", "SelectedItem é nulo.", "Okey");
                }
            } else {
                await DisplayAlert("ERRO", "CommandParameter é nulo.", "Okey");
            }
        } else {
            await DisplayAlert("ERRO", "Sender is not button", "Okey");
        }
    }

    private void mostrarItens(object sender, EventArgs e) {
        if (!itemVisivel) {
            collectionViewItem.IsVisible = true;
            itemVisivel = true;
        } else {
            collectionViewItem.IsVisible = false;
            itemVisivel = false;
        }
    }

    private async void focusEntry(object sender, EventArgs e) {
        var listOfChildren = layout.Children.ToList();
        foreach (var child in listOfChildren) {
            if (child is Entry entry) {
                var elemento = entry.BindingContext;
                await scroll.ScrollToAsync(elemento, ScrollToPosition.MakeVisible, true);
            }
        }
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        List<Planta> listaDePlantas = await CadastroUsuarioPage.Database.GetPlantsAsync();
        List<Planta> listaDePlantasPessoa = new List<Planta>();
        foreach (Planta planta in listaDePlantas) {
            if (planta.idPessoa == id) {
                listaDePlantasPessoa.Add(planta);
            }
        }
        List<Item> listaDeItens = await CadastroUsuarioPage.Database.GetItemAsync();
        List<Item> listaDeItensPessoa = new List<Item>();
        foreach (Item item in listaDeItens) {
            if (item.idPessoa == id) {
                listaDeItensPessoa.Add(item);
            }
        }
        collectionViewPlanta.ItemsSource = listaDePlantasPessoa;
        collectionViewItem.ItemsSource = listaDeItensPessoa;
    }
}