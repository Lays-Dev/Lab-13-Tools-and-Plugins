using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
// Gives acess to Unitys Networking tools. Like UnityWebRequest.
using UnityEngine.Networking;

// These are the five available cities.
// Unity displays this enum as a dropdown in the Inspector.
public enum CitySelection
{
    Orlando,
    London,
    Tokyo,
    Reykjavik,
    Sydney,
    NewYork
}

// This script will contact the weather API
public class WeatherManager : MonoBehaviour
{
   // OpenWeatherMap API key.
   public string apiKey = "YOUR-API-KEY";

   // The city we want to retrieve weather information for.
   [Header("City Selection")]

    // Select the city directly from the Inspector dropdown.
    public CitySelection selectedCity;

    // Stores the previously selected city.
    private CitySelection previousCity;


   [Header("Scene References")]
   public WeatherSceneController sceneController;


   private void Start()
   {
      // Remember which city was selected when the game started.
      previousCity = selectedCity;

      // Request weather for the selected city.
      StartCoroutine(GetWeather(GetCityName()));
   }


   private void Update()
   {
      // Check whether the selected city has changed.
      if (selectedCity != previousCity)
      {
         // Remember the new selection.
         previousCity = selectedCity;
         Debug.Log("City selection changed to: " + selectedCity);

         // Request weather for the newly selected city.
         StartCoroutine(GetWeather(GetCityName()));
      }
   }

   // Converts the Inspector dropdown selection into an API city name.
    private string GetCityName()
    {
        switch (selectedCity)
        {
            case CitySelection.Orlando:
                return "Orlando,US";

            case CitySelection.London:
                return "London,GB";

            case CitySelection.Tokyo:
                return "Tokyo,JP";

            case CitySelection.Reykjavik:
                return "Reykjavik,IS";

            case CitySelection.Sydney:
                return "Sydney,AU";
            
            case CitySelection.NewYork:
               return "New York,US";

            default:
                return "Orlando,US";
        }
    }

   private IEnumerator GetWeather(string cityName)
   {
      // EscapeURL makes sure that the city variable is formatted for a URL.
      string url = "https://api.openweathermap.org/data/2.5/weather?q=" + UnityWebRequest.EscapeURL(cityName) + "&units=metric&appid=" + apiKey;

      // Creates a request to retrueve information from a URL
      using (UnityWebRequest request = UnityWebRequest.Get(url)) 
      {
         // Send the HTTP request, wait until the request finishes, continue while waiting.
         yield return request.SendWebRequest();

         // Debug Checks
         if (request.result == UnityWebRequest.Result.ConnectionError) 
         {
            Debug.LogError($"network problem: {request.error}");
         } 
         else if (request.result == UnityWebRequest.Result.ProtocolError) 
         {
            Debug.LogError($"response error: {request.responseCode}");
         } 
         else 
         {
            // Callback method that recives the weather information.
            OnWeatherDataReceived(request.downloadHandler.text);
            // Send the information to the callback
            //callback(request.downloadHandler.text);
         }
      }
   }

   // Recieve the weather information



   public void OnWeatherDataReceived(string data)
   {
      WeatherData weather =
         JsonUtility.FromJson<WeatherData>(data);

      Debug.Log("CITY: " + weather.name);
      Debug.Log("TEMPERATURE: " + weather.main.temp + " °C");
      Debug.Log("WEATHER: " + weather.weather[0].main);
      Debug.Log("TIMEZONE: " + weather.timezone);

      // Get the current UTC time as a Unix timestamp.
      long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

      // Compare the current time against the city's sunrise and sunset.
      bool isDaytime =
         currentTime >= weather.sys.sunrise &&
         currentTime < weather.sys.sunset;

      // Convert the current time to the selected city's local time.
      TimeSpan cityOffset =
         TimeSpan.FromSeconds(weather.timezone);

      DateTimeOffset cityTime =
         DateTimeOffset.UtcNow.ToOffset(cityOffset);

      // Convert sunrise and sunset into the city's local time.
      DateTimeOffset sunriseTime =
         DateTimeOffset.FromUnixTimeSeconds(weather.sys.sunrise)
         .ToOffset(cityOffset);

      DateTimeOffset sunsetTime =
         DateTimeOffset.FromUnixTimeSeconds(weather.sys.sunset)
         .ToOffset(cityOffset);

      Debug.Log("LOCAL CITY TIME: " + cityTime.ToString("HH:mm"));
      Debug.Log("LOCAL SUNRISE: " + sunriseTime.ToString("HH:mm"));
      Debug.Log("LOCAL SUNSET: " + sunsetTime.ToString("HH:mm"));

      Debug.Log("IS DAYTIME: " + isDaytime);

      // Update the scene.
      if (sceneController != null)
      {
         sceneController.UpdateWeatherEnvironment(
               weather.weather[0].main
         );

         sceneController.UpdateTimeOfDay(isDaytime);
      }
      else
      {
         Debug.LogWarning("WeatherSceneController is not assigned!");
      }
   }




   /*private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Orlando,us&mode=xml&appid=APIKEY";

   private IEnumerator CallAPI(string url, Action<string> callback) 
   {
  
   }

   // IEnumerator allows a method to pause its execution and continue later.
   // Contacting a website takes time, and Unity would freeze while waiting for the Internet. 
   // Use a coroutine to avoid this. A coroutine allows Unity to execute a method over multiple frames.
   public IEnumerator GetWeatherXML(Action<string> callback) 
   {
      return CallAPI(xmlApi, callback);
   }
   */

}