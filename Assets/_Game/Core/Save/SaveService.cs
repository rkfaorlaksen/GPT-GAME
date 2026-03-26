using UnityEngine;

namespace MilkyWayStation.Core.Save
{
    public sealed class SaveService : ISaveService
    {
        private const string SaveKey = "milky_way_station.save";

        public GameState LoadGameState()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return new GameState();
            }

            var raw = PlayerPrefs.GetString(SaveKey);
            return string.IsNullOrWhiteSpace(raw)
                ? new GameState()
                : JsonUtility.FromJson<GameState>(raw) ?? new GameState();
        }

        public void SaveGameState(GameState state)
        {
            state.LastSavedUtcTicks = System.DateTime.UtcNow.Ticks;
            var raw = JsonUtility.ToJson(state);
            PlayerPrefs.SetString(SaveKey, raw);
            PlayerPrefs.Save();
        }
    }
}
