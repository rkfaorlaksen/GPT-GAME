using System;

namespace MilkyWayStation.Core.EventBus
{
    public sealed class GameEventBus
    {
        public event Action<int, string> CropHarvested;
        public event Action<string> TeaCrafted;
        public event Action<string, int> GuestAffinityChanged;

        public void PublishCropHarvested(int plotId, string cropId) => CropHarvested?.Invoke(plotId, cropId);
        public void PublishTeaCrafted(string teaItemId) => TeaCrafted?.Invoke(teaItemId);
        public void PublishGuestAffinityChanged(string npcId, int affinity) => GuestAffinityChanged?.Invoke(npcId, affinity);
    }
}
