using System.Collections.Generic;
using System.Linq;
using MilkyWayStation.Core.Save;

namespace MilkyWayStation.Systems.Inventory
{
    public sealed class InventorySystem
    {
        private readonly GameState _state;

        public InventorySystem(GameState state)
        {
            _state = state;
        }

        public void AddItem(string itemId, int qty)
        {
            if (qty <= 0) return;
            var entry = _state.Inventory.FirstOrDefault(x => x.ItemId == itemId);
            if (entry == null)
            {
                _state.Inventory.Add(new InventoryEntryState { ItemId = itemId, Quantity = qty });
                return;
            }

            entry.Quantity += qty;
        }

        public bool RemoveItem(string itemId, int qty)
        {
            if (qty <= 0) return false;
            var entry = _state.Inventory.FirstOrDefault(x => x.ItemId == itemId);
            if (entry == null || entry.Quantity < qty) return false;

            entry.Quantity -= qty;
            if (entry.Quantity <= 0)
            {
                _state.Inventory.Remove(entry);
            }

            return true;
        }

        public bool HasItems(IEnumerable<(string itemId, int qty)> requirements)
        {
            return requirements.All(req =>
            {
                var entry = _state.Inventory.FirstOrDefault(x => x.ItemId == req.itemId);
                return entry != null && entry.Quantity >= req.qty;
            });
        }
    }
}
