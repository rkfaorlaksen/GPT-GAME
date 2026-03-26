using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "MilkyWayStation/Data/EconomyConfig")]
    public sealed class EconomyConfig : ScriptableObject
    {
        [Range(0f, 1f)] public float WaterSpeedUpRate = 0.2f;
        public string FallbackTeaItemId = "tea_fallback_barley";
        public int AffinityGainPerfect = 2;
        public int AffinityGainDefault = 1;
        public int IdleFairyMaxHours = 24;
        [Min(3)] public int BrewMinSec = 3;
        [Min(5)] public int BrewMaxSec = 5;
    }
}
