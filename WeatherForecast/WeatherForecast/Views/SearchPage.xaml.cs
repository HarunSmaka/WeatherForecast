using System;
using WeatherForecast.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherForecast.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {


        public SearchPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            GetData getData = new GetData();
            string searchedLocation = NewLocationEntry.Text;

            if (await getData.LocationExists(searchedLocation))
            {
                await Shell.Current.Navigation.PushModalAsync(new CurrentWeatherPage(searchedLocation, "back.png"));

                NewLocationEntry.Text = String.Empty;
            }
            else
            {
                await DisplayAlert("Location not found", "Please check your input", "OK");
            }

        }
    }
}