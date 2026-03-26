using System;
using UnityEngine;

namespace MilkyWayStation.Core.Time
{
    public sealed class TimeService : ITimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public int GetInGameHour()
        {
            // MVP: 현실 시간 기반. 추후 배속/별도 월드시간 도입 가능.
            return DateTime.Now.Hour;
        }

        public WeatherType GetWeather()
        {
            // TODO: 기상 시스템 연동 전까지 기본값 반환.
            return WeatherType.Clear;
        }
    }
}
