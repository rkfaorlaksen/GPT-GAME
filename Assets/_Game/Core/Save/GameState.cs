using System;
using System.Collections.Generic;

namespace MilkyWayStation.Core.Save
{
    [Serializable]
    public sealed class GameState
    {
        public long LastSavedUtcTicks;
        public CurrencyState Currency = new();
        public List<InventoryEntryState> Inventory = new();
        public List<FarmPlotState> FarmPlots = new();
        public List<NpcGuestState> Guests = new();
        public List<FurniturePlacementState> FurniturePlacements = new();
        public List<string> UnlockedZones = new();
        public List<string> UnlockedRecipeIds = new();
    }

    [Serializable]
    public sealed class CurrencyState
    {
        public int GalaxyDust;
        public int StarShard;
    }

    [Serializable]
    public sealed class InventoryEntryState
    {
        public string ItemId;
        public int Quantity;
    }

    [Serializable]
    public sealed class FarmPlotState
    {
        public int PlotId;
        public string CropId;
        public long PlantedAtUtcTicks;
        public long BaseFinishAtUtcTicks;
        public bool Watered;
        public long FinishAtUtcTicks;

        public bool IsReady(DateTime utcNow)
        {
            return !string.IsNullOrEmpty(CropId) && utcNow.Ticks >= FinishAtUtcTicks;
        }
    }

    [Serializable]
    public sealed class NpcGuestState
    {
        public string NpcId;
        public int Affinity;
        public string CurrentMoodTag;
        public long LastVisitUtcTicks;
        public List<string> UnlockedPostcards = new();
    }

    [Serializable]
    public sealed class FurniturePlacementState
    {
        public string FurnitureId;
        public int GridX;
        public int GridY;
        public int Rotation;
    }
}
