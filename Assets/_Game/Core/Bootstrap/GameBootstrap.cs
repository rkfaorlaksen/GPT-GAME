using System;
using MilkyWayStation.Core.EventBus;
using MilkyWayStation.Core.Save;
using MilkyWayStation.Core.Time;
using MilkyWayStation.Data.Definitions;
using MilkyWayStation.Systems.Blending;
using MilkyWayStation.Systems.Farm;
using MilkyWayStation.Systems.Housing;
using MilkyWayStation.Systems.IdleFairy;
using MilkyWayStation.Systems.Inventory;
using MilkyWayStation.Systems.NPC;
using MilkyWayStation.UI.StateMachine;
using UnityEngine;

namespace MilkyWayStation.Core.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private EconomyConfig economyConfig;
        [SerializeField] private CropDefinition[] cropDefinitions;
        [SerializeField] private RecipeDefinition[] recipeDefinitions;
        [SerializeField] private NpcDefinition[] npcDefinitions;

        private ISaveService _saveService;
        private ITimeService _timeService;
        private GameState _state;

        private void Awake()
        {
            _saveService = new SaveService();
            _timeService = new TimeService();

            _state = _saveService.LoadGameState();
            var eventBus = new GameEventBus();
            var inventory = new InventorySystem(_state);
            var farm = new FarmSystem(_state, _timeService, inventory, eventBus, cropDefinitions);
            var blending = new BlendingSystem(inventory, eventBus, economyConfig, recipeDefinitions);
            var npc = new NpcSystem(_state, _timeService, eventBus, economyConfig, npcDefinitions);
            var housing = new HousingSystem(_state);
            var idleFairy = new IdleFairySystem(_state, economyConfig);
            var uiStateMachine = new UiStateMachine();

            var lastSavedTicks = _state.LastSavedUtcTicks == 0 ? _timeService.UtcNow.Ticks : _state.LastSavedUtcTicks;
            var elapsed = new DateTime(_timeService.UtcNow.Ticks) - new DateTime(lastSavedTicks);
            idleFairy.SimulateOfflineCleanup(elapsed);
            farm.RefreshOfflineProgress(elapsed);

            _ = blending;
            _ = npc;
            _ = housing;
            _ = uiStateMachine;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Persist();
        }

        private void OnApplicationQuit() => Persist();

        private void Persist()
        {
            if (_state != null)
            {
                _saveService.SaveGameState(_state);
            }
        }
    }
}
