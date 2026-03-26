using System;
using System.Collections.Generic;
using System.Linq;
using MilkyWayStation.Core.EventBus;
using MilkyWayStation.Core.Save;
using MilkyWayStation.Core.Time;
using MilkyWayStation.Data.Definitions;
using MilkyWayStation.Systems.Inventory;

namespace MilkyWayStation.Systems.Farm
{
    public sealed class FarmSystem
    {
        private readonly GameState _state;
        private readonly ITimeService _timeService;
        private readonly InventorySystem _inventory;
        private readonly GameEventBus _eventBus;
        private readonly Dictionary<string, CropDefinition> _cropMap;

        public FarmSystem(
            GameState state,
            ITimeService timeService,
            InventorySystem inventory,
            GameEventBus eventBus,
            IEnumerable<CropDefinition> crops)
        {
            _state = state;
            _timeService = timeService;
            _inventory = inventory;
            _eventBus = eventBus;
            _cropMap = crops.ToDictionary(x => x.Id, x => x);
        }

        public bool Plant(int plotId, string cropId)
        {
            if (!_cropMap.TryGetValue(cropId, out var crop)) return false;
            var nowTicks = _timeService.UtcNow.Ticks;
            var finishTicks = _timeService.UtcNow.AddSeconds(crop.GrowthDurationSec).Ticks;

            var plot = GetOrCreatePlot(plotId);
            plot.CropId = cropId;
            plot.PlantedAtUtcTicks = nowTicks;
            plot.BaseFinishAtUtcTicks = finishTicks;
            plot.FinishAtUtcTicks = finishTicks;
            plot.Watered = false;
            return true;
        }

        public bool Water(int plotId)
        {
            var plot = _state.FarmPlots.FirstOrDefault(x => x.PlotId == plotId);
            if (plot == null || string.IsNullOrEmpty(plot.CropId) || plot.Watered) return false;
            if (!_cropMap.TryGetValue(plot.CropId, out var crop)) return false;

            var remainingTicks = plot.FinishAtUtcTicks - _timeService.UtcNow.Ticks;
            if (remainingTicks <= 0) return false;

            var reduced = (long)(remainingTicks * crop.WaterSpeedUpRate);
            plot.FinishAtUtcTicks -= reduced;
            plot.Watered = true;
            return true;
        }

        public bool HarvestBySwipe(int plotId)
        {
            var plot = _state.FarmPlots.FirstOrDefault(x => x.PlotId == plotId);
            if (plot == null || !plot.IsReady(_timeService.UtcNow) || !_cropMap.TryGetValue(plot.CropId, out var crop))
            {
                return false;
            }

            foreach (var yield in crop.HarvestYield)
            {
                _inventory.AddItem(yield.ItemId, yield.Quantity);
            }

            var harvestedCrop = plot.CropId;
            plot.CropId = null;
            plot.Watered = false;
            _eventBus.PublishCropHarvested(plotId, harvestedCrop);
            return true;
        }

        public void RefreshOfflineProgress(TimeSpan elapsed)
        {
            // Real-time 성장이라 별도 누적 계산보다 ready 상태만 재평가하면 충분.
            _ = elapsed;
        }

        private FarmPlotState GetOrCreatePlot(int plotId)
        {
            var plot = _state.FarmPlots.FirstOrDefault(x => x.PlotId == plotId);
            if (plot != null) return plot;

            plot = new FarmPlotState { PlotId = plotId };
            _state.FarmPlots.Add(plot);
            return plot;
        }
    }
}
