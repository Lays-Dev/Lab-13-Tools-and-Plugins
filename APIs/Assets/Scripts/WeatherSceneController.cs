using UnityEngine;

public class WeatherSceneController : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sun;

    [Header("Skybox Materials")]
    
    public Material daySkybox;
    public Material nightSkybox;
    public Material sunnySkybox;
    public Material cloudySkybox;
    public Material mistSkybox;
    public Material rainySkybox;
    public Material snowySkybox;

    // Stores the weather condition received from the API.
    private string currentCondition = "clear";

    // Tracks whether the selected city is currently at night.
    private bool isNight = false;

    // Called when new weather data is received.
    public void UpdateWeatherEnvironment(string condition)
    {
        currentCondition = condition.ToLower();
    }

    // Called when the city's time of day is calculated.
    public void UpdateTimeOfDay(bool isDaytime)
    {
        isNight = !isDaytime;
    }

    // Combines the weather and time of day to update the scene.
    public void ApplyEnvironment()
    {
        // Nighttime takes priority over daytime weather.
        if (isNight)
        {
            SetSkybox(nightSkybox);
            SetSun(0.1f, new Color(0.25f, 0.3f, 0.5f));

            Debug.Log("Scene updated to NIGHT.");

            return;
        }

        // If it is daytime, choose the skybox and lighting
        // based on the current weather condition.
        if (currentCondition == "clear")
        {
            SetSkybox(sunnySkybox);
            SetSun(1.5f, new Color(1f, 0.85f, 0.55f));
        }
        else if (currentCondition == "clouds")
        {
            SetSkybox(cloudySkybox);
            SetSun(0.8f, Color.white);
        }
        else if (currentCondition == "rain" ||
                 currentCondition == "drizzle" ||
                 currentCondition == "thunderstorm")
        {
            SetSkybox(rainySkybox);
            SetSun(0.4f, new Color(0.65f, 0.7f, 0.8f));
        }
        else if (currentCondition == "snow")
        {
            SetSkybox(snowySkybox);
            SetSun(0.9f, new Color(0.8f, 0.85f, 1f));
        }
        else if (currentCondition == "mist" ||
                 currentCondition == "fog" ||
                 currentCondition == "haze")
        {
            SetSkybox(mistSkybox);
            SetSun(0.5f, Color.gray);
        }
        else
        {
            SetSkybox(daySkybox);
            SetSun(1f, Color.white);
        }

        Debug.Log("Scene updated to DAY. Weather: " + currentCondition);
    }

    private void SetSkybox(Material selectedSkybox)
    {
        if (selectedSkybox == null)
        {
            Debug.LogWarning("No skybox material assigned!");
            return;
        }

        RenderSettings.skybox = selectedSkybox;
        DynamicGI.UpdateEnvironment();
    }

    private void SetSun(float intensity, Color color)
    {
        if (sun == null)
        {
            Debug.LogWarning("No Directional Light assigned!");
            return;
        }

        sun.intensity = intensity;
        sun.color = color;
    }
}

