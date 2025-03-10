
using System.Collections.ObjectModel;
using Tarea6AMAM.Services;

namespace Tarea6.Vista.ContainerViews;

public partial class PokemonPage : ContentPage
{
    private readonly httpconexion _apiService;
    public ObservableCollection<string> Abilities { get; set; }

    public PokemonPage()
    {
        InitializeComponent();
        _apiService = new httpconexion();
        Abilities = new ObservableCollection<string>();
        AbilitiesCollectionView.ItemsSource = Abilities;
    }

    private async void OnSearchPokemonClicked(object sender, EventArgs e)
    {
        string pokemonName = PokemonEntry.Text?.Trim().ToLower();
        if (string.IsNullOrEmpty(pokemonName))
        {
            await DisplayAlert("Error", "Ingrese el nombre de un Pokémon.", "Entendido");
            return;
        }

        string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{pokemonName}";

        try
        {
            var pokemonData = await _apiService.GetAsync<PokemonResponse>(apiUrl);

            PokemonNameLabel.Text = pokemonData.Name.ToUpper();
            BaseExperienceLabel.Text = $"Experiencia Base: {pokemonData.BaseExperience}";

            if (!string.IsNullOrEmpty(pokemonData.Sprites?.FrontDefault))
            {
                PokemonImage.Source = ImageSource.FromUri(new Uri(pokemonData.Sprites.FrontDefault));
                PokemonImage.IsVisible = true;
            }
            else
            {
                PokemonImage.IsVisible = false;
                await DisplayAlert("Error", "No se consiguio foto para este Pokémon.", "Entendido");
            }


            Abilities.Clear();
            foreach (var ability in pokemonData.Abilities)
            {
                Abilities.Add(ability.Ability.Name.ToUpper());
            }

            PokemonImage.IsVisible = true;
            PokemonNameLabel.IsVisible = true;
            BaseExperienceLabel.IsVisible = true;
            AbilitiesHeader.IsVisible = true;
            AbilitiesCollectionView.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se encontro el Pokémon: {ex.Message}", "Entendido");
        }
    }


    public class PokemonResponse
    {
        public string Name { get; set; }
        public int BaseExperience { get; set; }
        public List<PokemonAbility> Abilities { get; set; }
        public PokemonSprites Sprites { get; set; }
    }

    public class PokemonSprites
    {
        public string FrontDefault { get; set; }
    }

    public class PokemonAbility
    {
        public AbilityInfo Ability { get; set; }
    }

    public class AbilityInfo
    {
        public string Name { get; set; }
    }

}