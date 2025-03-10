



using Tarea6AMAM.Services;

namespace Tarea6.Vista.ContainerViews;

public partial class GeneroPage : ContentPage
{
    private readonly httpconexion _apiService;

    public GeneroPage()
    {
        InitializeComponent();
        _apiService = new httpconexion();
        BindingContext = this;
    }


    private async void OnPredictGenderClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Ocurrio un Problema, intente de nuevo.", "Entendido");
            return;
        }

        string apiUrl = $"https://api.genderize.io/?name={name}";

        try
        {
            var genderResponse = await _apiService.GetAsync<GenderResponse>(apiUrl);

            if (genderResponse?.Gender == "male")
            {
                BackgroundColor = Color.FromArgb("#0022e0");
                ResultLabel.Text = $"El género de {name} es Masculino";
            }
            else if (genderResponse?.Gender == "female")
            {
                BackgroundColor = Color.FromArgb("#d400b1"); // Rosa para femenino
                ResultLabel.Text = $"El género de {name} es Femenino";
            }
            else
            {
                BackgroundColor = Color.FromArgb("#757575"); // Gris si no se encuentra
                ResultLabel.Text = $"No se pudo reconocer el genero de: {name}";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrio un Problema, intente de nuevo: {ex.Message}", "Entendido");
        }
    }
}
public class GenderResponse
{
    public string Gender { get; set; }
}