using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Погодное приложение");
        Console.WriteLine("-------------------");

        string apiKey = "ae614215e9893f40de0437d8bf669ad0";

        Console.Write("Введите название города: ");
        string city = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("Название города не может быть пустым.");
            return;
        }

        try
        {
            var weatherData = await GetWeatherData(city, apiKey);
            DisplayWeather(weatherData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static async Task<WeatherData> GetWeatherData(string city, string apiKey)
    {
        using (var client = new HttpClient())
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ru";

            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при запросе к API: {response.StatusCode}");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherData>(json);
        }
    }

    static void DisplayWeather(WeatherData weather)
    {
        Console.WriteLine("\nТекущая погода:");
        Console.WriteLine($"Город: {weather.Name}");
        Console.WriteLine($"Температура: {weather.Main.Temp} °C");
        Console.WriteLine($"Ощущается как: {weather.Main.FeelsLike} °C");
        Console.WriteLine($"Влажность: {weather.Main.Humidity}%");
        Console.WriteLine($"Погодные условия: {weather.Weather[0].Description}");
        Console.WriteLine($"Скорость ветра: {weather.Wind.Speed} м/с");
    }
}

public class WeatherData
{
    public string Name { get; set; }
    public MainData Main { get; set; }
    public WeatherDescription[] Weather { get; set; }
    public WindData Wind { get; set; }
}

public class MainData
{
    public float Temp { get; set; }
    public float FeelsLike { get; set; }
    public int Humidity { get; set; }
}

public class WeatherDescription
{
    public string Description { get; set; }
}

public class WindData
{
    public float Speed { get; set; }
}