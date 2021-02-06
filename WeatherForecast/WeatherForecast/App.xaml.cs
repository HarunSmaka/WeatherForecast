using Xamarin.Forms;

namespace WeatherForecast
{
    public partial class App : Application
    {
        public static string FilePath;

        public App(string filePath)
        {
            InitializeComponent();

            MainPage = new AppShell(filePath);

            FilePath = filePath;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
