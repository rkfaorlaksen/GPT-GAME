using System;

namespace MilkyWayStation.Core.Time
{
    public interface ITimeService
    {
        DateTime UtcNow { get; }
        int GetInGameHour();
        WeatherType GetWeather();
    }

    public enum WeatherType
    {
        Clear,
        Cloudy,
        Rain,
        Snow
    }
}
