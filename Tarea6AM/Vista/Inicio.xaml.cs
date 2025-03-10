using System.Threading.Tasks;
using Tarea6.Vista.ContainerViews;


namespace Tarea6AM.Vista;

public partial class Presentation : ContentPage
{
	public Presentation()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        await imagenHerramientas.FadeTo(1, 500);
        await imagenHerramientas.FadeTo(0, 500);
        await imagenHerramientas.FadeTo(1, 500);


        Application.Current.MainPage = new Container();
    }


}