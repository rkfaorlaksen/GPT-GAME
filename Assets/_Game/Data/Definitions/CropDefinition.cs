using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "CropDefinition", menuName = "MilkyWayStation/Data/Crop")]
    public sealed class CropDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        [Min(600)] public int GrowthDurationSec = 600;
        [Range(0f, 1f)] public float WaterSpeedUpRate = 0.2f;
        public ItemStack[] HarvestYield;
    }

    [System.Serializable]
    public struct ItemStack
    {
        public string ItemId;
        public int Quantity;
    }
}
