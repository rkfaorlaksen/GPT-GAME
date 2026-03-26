using UnityEngine;

namespace MilkyWayStation.Data.Definitions
{
    [CreateAssetMenu(fileName = "RecipeDefinition", menuName = "MilkyWayStation/Data/Recipe")]
    public sealed class RecipeDefinition : ScriptableObject
    {
        public string Id;
        public string BaseIngredientId;
        public string IngredientAId;
        public string IngredientBId;
        public string ResultItemId;
        [TextArea] public string FlavorText;
        public string[] TargetMoodTags;
    }
}
