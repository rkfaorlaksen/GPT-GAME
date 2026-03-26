using System;
using System.Collections.Generic;
using System.Linq;
using MilkyWayStation.Core.EventBus;
using MilkyWayStation.Core.Save;
using MilkyWayStation.Core.Time;
using MilkyWayStation.Data.Definitions;

namespace MilkyWayStation.Systems.NPC
{
    public sealed class NpcSystem
    {
        private readonly GameState _state;
        private readonly ITimeService _timeService;
        private readonly GameEventBus _eventBus;
        private readonly EconomyConfig _economy;
        private readonly Dictionary<string, NpcDefinition> _npcMap;

        public NpcSystem(
            GameState state,
            ITimeService timeService,
            GameEventBus eventBus,
            EconomyConfig economy,
            IEnumerable<NpcDefinition> npcDefinitions)
        {
            _state = state;
            _timeService = timeService;
            _eventBus = eventBus;
            _economy = economy;
            _npcMap = npcDefinitions.ToDictionary(x => x.Id, x => x);
        }

        public string SpawnGuestByContext()
        {
            // MVP: 랜덤 스폰. 추후 시간/날씨 가중치 반영.
            return _npcMap.Keys.FirstOrDefault();
        }

        public int ServeTea(string npcId, bool moodMatched)
        {
            var guest = GetOrCreateGuestState(npcId);
            guest.Affinity += moodMatched ? _economy.AffinityGainPerfect : _economy.AffinityGainDefault;
            guest.Affinity = Math.Clamp(guest.Affinity, 0, 100);
            guest.LastVisitUtcTicks = _timeService.UtcNow.Ticks;

            _eventBus.PublishGuestAffinityChanged(npcId, guest.Affinity);
            return guest.Affinity;
        }

        public bool CheckPostcardUnlock(string npcId)
        {
            var guest = GetOrCreateGuestState(npcId);
            if (guest.Affinity < 100 || !_npcMap.TryGetValue(npcId, out var npc)) return false;
            if (guest.UnlockedPostcards.Contains(npc.PostcardId)) return false;

            guest.UnlockedPostcards.Add(npc.PostcardId);
            return true;
        }

        private NpcGuestState GetOrCreateGuestState(string npcId)
        {
            var state = _state.Guests.FirstOrDefault(x => x.NpcId == npcId);
            if (state != null) return state;

            state = new NpcGuestState { NpcId = npcId, CurrentMoodTag = "neutral" };
            _state.Guests.Add(state);
            return state;
        }
    }
}
