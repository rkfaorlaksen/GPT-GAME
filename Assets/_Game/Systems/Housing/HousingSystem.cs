using System.Linq;
using MilkyWayStation.Core.Save;

namespace MilkyWayStation.Systems.Housing
{
    public sealed class HousingSystem
    {
        private readonly GameState _state;

        public HousingSystem(GameState state)
        {
            _state = state;
        }

        public bool PlaceFurniture(int gridX, int gridY, string furnitureId, int rotation)
        {
            var occupied = _state.FurniturePlacements.Any(x => x.GridX == gridX && x.GridY == gridY);
            if (occupied) return false;

            _state.FurniturePlacements.Add(new FurniturePlacementState
            {
                FurnitureId = furnitureId,
                GridX = gridX,
                GridY = gridY,
                Rotation = rotation
            });
            return true;
        }

        public void UnlockZone(string zoneId)
        {
            if (_state.UnlockedZones.Contains(zoneId)) return;
            _state.UnlockedZones.Add(zoneId);
        }
    }
}
