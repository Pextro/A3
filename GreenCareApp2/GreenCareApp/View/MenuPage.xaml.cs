using Microsoft.Maui.Controls;
using GreenCareApp.entities;

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

    private void mostrarItens(object sender, EventArgs e) {
        if (!itemVisivel) {
            collectionViewItem.IsVisible = true;
            itemVisivel = true;
        } else {
            collectionViewItem.IsVisible = false;
            itemVisivel = false;
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