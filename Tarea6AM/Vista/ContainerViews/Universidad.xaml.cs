using Tarea6AMAM.Services;
using System.Collections.ObjectModel;


namespace Tarea6.Vista.ContainerViews;

public partial class UniversidadPage : ContentPage
{

    private readonly httpconexion _apiService;
    public ObservableCollection<University> Universities { get; set; }

    public UniversidadPage()
    {
        InitializeComponent();
        _apiService = new httpconexion();
        Universities = new ObservableCollection<University>();
        BindingContext = this;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        string country = CountryEntry.Text?.Trim();
        if (string.IsNullOrEmpty(country))
        {
            await DisplayAlert("Error", "Por favor, ingrese un país en inglés.", "Entendido");
            return;
        }

        string formattedCountry = country.Replace(" ", "+");
        string apiUrl = $"http://universities.hipolabs.com/search?country={formattedCountry}";

        try
        {
            var universities = await _apiService.GetAsync<University[]>(apiUrl);

            Universities.Clear();
            foreach (var uni in universities)
            {
                Universities.Add(new University
                {
                    Name = uni.Name,
                    Domain = uni.Domains?.FirstOrDefault() ?? "No disponible",
                    WebPage = uni.web_pages?.FirstOrDefault() ?? "#"
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema al obtener las universidades: {ex.Message}", "Entendido");
        }
    }

    private async void OnVisitWebsite(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string url && url != "#")
        {
            await Launcher.OpenAsync(url);
        }
    }

    public class University
    {
        public string Name { get; set; }
        public string[] Domains { get; set; }
        public string[] web_pages { get; set; }
        public string Domain { get; set; }
        public string WebPage { get; set; }
    }
}