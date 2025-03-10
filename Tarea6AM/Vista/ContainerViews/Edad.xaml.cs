
using Tarea6AMAM.Services;

namespace Tarea6.Vista.ContainerViews;

public partial class Edad : ContentPage
{

    private readonly httpconexion _apiService;

    public Edad()
    {
        InitializeComponent();
        _apiService = new httpconexion();
        BindingContext = this;
    }

    private async void OnPredictAgeClicked(object sender, EventArgs e)
    {
        
        string name = NameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Nombre inválido, intente de nuevo.", "Entendido");
            return;
        }


        string apiUrl = $"https://api.agify.io/?name={name}";

        try
        {
            var ageResponse = await _apiService.GetAsync<AgeResponse>(apiUrl);

            if (ageResponse?.Age == null)
            {
                await DisplayAlert("Error", "Ocurrio un problema al predecir la edad.", "Entendido");
                return;
            }

            int age = ageResponse.Age.Value;
            string category;
            string imageUrl;

            if (age < 18)
            {
                category = "Joven";
                imageUrl = "chico.png";
            }
            else if (age < 60)
            {
                category = "Adulto";
                imageUrl = "par.png";
            }
            else
            {
                category = "Anciano";
                imageUrl = "anciano.png";
            }

            ResultLabel.Text = $"Edad de {name}: {age} años, es {category}";
            PersonImage.Source = ImageSource.FromFile(imageUrl);
            PersonImage.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrio un problema: {ex.Message}", "Entendido");
        }
    }
    private class AgeResponse
    {
        public int? Age { get; set; }
    }
}