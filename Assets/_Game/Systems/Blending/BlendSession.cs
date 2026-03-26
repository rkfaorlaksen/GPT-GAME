using System.Collections.Generic;

namespace MilkyWayStation.Systems.Blending
{
    public sealed class BlendSession
    {
        public List<string> SelectedIngredientIds { get; } = new();
        public BlendState State { get; set; } = BlendState.Idle;
        public int BrewDurationSec { get; set; }
    }

    public enum BlendState
    {
        Idle,
        Brewing,
        Completed
    }
}
