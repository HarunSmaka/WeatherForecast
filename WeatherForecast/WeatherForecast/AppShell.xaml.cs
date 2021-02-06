using SQLite;
using System.Collections.Generic;
using WeatherForecast.Models;
using WeatherForecast.Views;
using Xamarin.Forms;

namespace WeatherForecast
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        List<LocationModel> locationList = new List<LocationModel>();
        public AppShell(string FilePath)
        {
            InitializeComponent();

            using (var connection = new SQLiteConnection(FilePath))
            {
                connection.CreateTable<LocationModel>();
                locationList = connection.Table<LocationModel>().ToList();
            }


            foreach (var location in locationList)
            {
                ShellSection shell_section = new ShellSection
                {
                    Title = location.LocationName
                };

                shell_section.Items.Add(new ShellContent() { Content = new CurrentWeatherPage(location.LocationName) });

                myshell.Items.Add(shell_section);
            }


            BindingContext = this;
        }
        public static void AddLocationToShell(string loc)
        {
            ShellSection shell_section = new ShellSection
            {
                Title = loc
            };

            shell_section.Items.Add(new ShellContent() { Content = new CurrentWeatherPage(loc) });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
