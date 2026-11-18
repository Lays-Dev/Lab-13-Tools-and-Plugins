using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
// Gives acess to Unitys Networking tools. Like UnityWebRequest.
using UnityEngine.Networking;

// This script will contact the weather API

public class WeatherManager : MonoBehaviour
{
   // OpenWeatherMap API key.
   public string apiKey = "YOUR-API-KEY";

   // The city we want to retrieve weather information for.
   public string city = "Orlando";

   private void Start()
   {
      StartCoroutine(GetWeather());
   }

   private IEnumerator GetWeather()
   {
      string url = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=" + apiKey;

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

            OnWeatherDataReceived(request.downloadHandler.text);
            // Send the information to the callback
            //callback(request.downloadHandler.text);
         }
      }
   }

   // Recieve the weather information
   public void OnWeatherDataReceived(string data)
   {
      Debug.Log("WEATHER DATA RECEIVED:");
      Debug.Log(data);
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