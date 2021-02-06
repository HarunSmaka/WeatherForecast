using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecast.Helper;
using WeatherForecast.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherForecast.Views
{

    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();

            GetCoordinates();
        }

        public CurrentWeatherPage(string city)
        {
            InitializeComponent();

            GetData getData = new GetData();
            Location = city;

            GetWeatherInfo();
            GetForecast();

            ImageButton deleteBtn = new ImageButton()
            {
                Source = "delete2.png",
                HeightRequest = 40,
                WidthRequest = 40,
                Margin = 10,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.Transparent
            };

            deleteBtn.Clicked += deleteBtn_Clicked;
            mainGrid.Children.Add(deleteBtn);
        }
        public CurrentWeatherPage(string city, string backBtnSource)
        {
            InitializeComponent();

            GetData getData = new GetData();
            Location = city;

            GetWeatherInfo();
            GetForecast();

            ImageButton backBtn = new ImageButton()
            {
                Source = backBtnSource,
                HeightRequest = 40,
                WidthRequest = 40,
                Margin = 10,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.Transparent
            };

            backBtn.Clicked += backBtn_Clicked;
            mainGrid.Children.Add(backBtn);
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        {

            using (var connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<LocationModel>();

                var rowsAffected = connection.Query<LocationModel>($"DELETE FROM LocationModel WHERE LocationName = '{Location}'");
                var currentShell = AppShell.Current.CurrentItem;

                DisplayAlert(Location, $"{Location} is removed from the list.", "OK");

                AppShell.Current.Items.Remove(currentShell);
            }
        }

        private string Location { get; set; } = "Berlin";
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);

                    GetWeatherInfo();
                    GetForecast();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
            }
        }

        private async Task<string> GetCity(Xamarin.Essentials.Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {
                return $"{currentPlace.Locality}, {currentPlace.CountryName}";
            }

            return "London, United Kingdom";
        }

        private async void GetWeatherInfo()
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Location}&appid=57fa57ff52ef2cd7db37b5577cd46a64&units=metric";

            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                    humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                    pressureTxt.Text = $"{weatherInfo.main.pressure} hpa";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

                    var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToUniversalTime().AddSeconds(weatherInfo.dt);

                    dateTxt.Text = dt.ToString("dddd, MMM dd").ToUpper();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "Weather information not found", "OK");
            }
        }

        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=57fa57ff52ef2cd7db37b5577cd46a64&units=metric";

            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                        {
                            allList.Add(list);
                        }

                    }

                    int allListCount = allList.Count;

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0");

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }
    }
}