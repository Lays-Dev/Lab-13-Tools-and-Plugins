using System;

// JSON parsing process. Extracting individual values.

[Serializable]
public class WeatherData
{
    // City name.
    public string name;

    // Temperature and humidity information.
    public MainData main;

    // Weather condition information.
    public WeatherCondition[] weather;

    // City's time difference from UTC, in seconds.
    public int timezone;

    // Time when the weather data was calculated.
    public long dt;

    // Sunrise and sunset information.
    public SystemData sys;
}

[Serializable]
public class MainData
{
    public float temp;
    public int humidity;
}

[Serializable]
public class WeatherCondition
{
    public string main;
    public string description;
}

[Serializable]
public class SystemData
{
    public long sunrise;
    public long sunset;
}