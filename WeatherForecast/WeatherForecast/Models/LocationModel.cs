using SQLite;

namespace WeatherForecast.Models
{
    public class LocationModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string LocationName { get; set; }
    }
}
