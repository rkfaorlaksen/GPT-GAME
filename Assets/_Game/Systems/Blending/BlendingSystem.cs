using System;
using System.Collections.Generic;
using System.Linq;
using MilkyWayStation.Core.EventBus;
using MilkyWayStation.Data.Definitions;
using MilkyWayStation.Systems.Inventory;

namespace MilkyWayStation.Systems.Blending
{
    public sealed class BlendingSystem
    {
        private readonly InventorySystem _inventory;
        private readonly GameEventBus _eventBus;
        private readonly EconomyConfig _economyConfig;
        private readonly List<RecipeDefinition> _recipes;
        private readonly Random _random = new();

        public BlendingSystem(
            InventorySystem inventory,
            GameEventBus eventBus,
            EconomyConfig economyConfig,
            IEnumerable<RecipeDefinition> recipes)
        {
            _inventory = inventory;
            _eventBus = eventBus;
            _economyConfig = economyConfig;
            _recipes = recipes.ToList();
        }

        public bool StartBlend(BlendSession session, IReadOnlyList<string> ingredientIds)
        {
            if (ingredientIds == null || ingredientIds.Count is 0 or > 3) return false;

            session.SelectedIngredientIds.Clear();
            session.SelectedIngredientIds.AddRange(ingredientIds);
            session.BrewDurationSec = _random.Next(_economyConfig.BrewMinSec, _economyConfig.BrewMaxSec + 1);
            session.State = BlendState.Brewing;
            return true;
        }

        public (string resultItemId, string flavorText) CompleteBlend(BlendSession session)
        {
            if (session.State != BlendState.Brewing)
            {
                return (string.Empty, string.Empty);
            }

            foreach (var itemId in session.SelectedIngredientIds)
            {
                _inventory.RemoveItem(itemId, 1);
            }

            var recipe = ResolveRecipe(session.SelectedIngredientIds);
            var result = recipe != null
                ? (recipe.ResultItemId, recipe.FlavorText)
                : (_economyConfig.FallbackTeaItemId, "특별하진 않지만 언제 마셔도 든든합니다.");

            _inventory.AddItem(result.Item1, 1);
            _eventBus.PublishTeaCrafted(result.Item1);
            session.State = BlendState.Completed;
            return result;
        }

        public RecipeDefinition ResolveRecipe(IReadOnlyList<string> ingredientIds)
        {
            if (ingredientIds == null || ingredientIds.Count < 2) return null;

            var normalized = ingredientIds.OrderBy(x => x).ToArray();
            foreach (var recipe in _recipes)
            {
                var recipeIngredients = new List<string> { recipe.BaseIngredientId, recipe.IngredientAId };
                if (!string.IsNullOrWhiteSpace(recipe.IngredientBId))
                {
                    recipeIngredients.Add(recipe.IngredientBId);
                }

                if (normalized.SequenceEqual(recipeIngredients.OrderBy(x => x)))
                {
                    return recipe;
                }
            }

            return null;
        }
    }
}
