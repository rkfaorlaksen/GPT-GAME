using System;
using MilkyWayStation.Core.Save;
using MilkyWayStation.Data.Definitions;

namespace MilkyWayStation.Systems.IdleFairy
{
    public sealed class IdleFairySystem
    {
        private readonly GameState _state;
        private readonly EconomyConfig _economy;

        public IdleFairySystem(GameState state, EconomyConfig economy)
        {
            _state = state;
            _economy = economy;
        }

        public int SimulateOfflineCleanup(TimeSpan elapsed)
        {
            var cappedHours = Math.Min(elapsed.TotalHours, _economy.IdleFairyMaxHours);
            var reward = (int)Math.Floor(cappedHours * 5); // MVP 임시 수익량
            _state.Currency.GalaxyDust += reward;
            return reward;
        }
    }
}
