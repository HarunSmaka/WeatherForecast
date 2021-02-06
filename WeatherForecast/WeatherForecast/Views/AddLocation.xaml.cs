using SQLite;
using System;
using WeatherForecast.Helper;
using WeatherForecast.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WeatherForecast.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLocation : ContentPage
    {
        public AddLocation()
        {
            InitializeComponent();
        }

        private async void newCityBtn_Clicked(object sender, EventArgs e)
        {
            GetData getData = new GetData();

            string cityName = newCity.Text.ToLower();
            cityName = char.ToUpper(cityName[0]) + cityName.Substring(1);

            if (await getData.LocationExists(newCity.Text))
            {
                var locationModel = new LocationModel()
                {
                    LocationName = cityName
                };

                using (var connection = new SQLiteConnection(App.FilePath))
                {
                    connection.CreateTable<LocationModel>();

                    int rowsAffected = connection.Insert(locationModel);
                }

                await DisplayAlert(cityName, "Location successfully added to the list", "OK");


                ShellSection shell_section = new ShellSection
                {
                    Title = newCity.Text
                };
                shell_section.Items.Add(new ShellContent() { Content = new CurrentWeatherPage(newCity.Text) });

                AppShell.Current.Items.Add(shell_section);

                newCity.Text = String.Empty;
            }
            else
            {
                await DisplayAlert("Location not found", "Please check your input", "OK");
            }
        }
    }
}