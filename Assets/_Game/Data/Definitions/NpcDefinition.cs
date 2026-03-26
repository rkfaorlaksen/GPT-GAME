using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "NpcDefinition", menuName = "MilkyWayStation/Data/NPC")]
    public sealed class NpcDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public string[] PreferredMoodTags;
        public string[] ActiveHours; // e.g. "Night", "Dawn"
        public string[] PreferredWeatherTags;
        public string PostcardId;
    }
}
