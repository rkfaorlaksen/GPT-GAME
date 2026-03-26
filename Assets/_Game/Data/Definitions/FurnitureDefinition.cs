using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "FurnitureDefinition", menuName = "MilkyWayStation/Data/Furniture")]
    public sealed class FurnitureDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public int CostStarShard;
        public int CostGalaxyDust;
        public Vector2Int Size = Vector2Int.one;
        public string InteractionAnimKey;
    }
}
