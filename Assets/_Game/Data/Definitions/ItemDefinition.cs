using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "MilkyWayStation/Data/Item")]
    public sealed class ItemDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public ItemType Type;
        public bool Stackable = true;
    }

    public enum ItemType
    {
        CropIngredient,
        TeaBase,
        TeaProduct,
        Material,
        Furniture
    }
}
