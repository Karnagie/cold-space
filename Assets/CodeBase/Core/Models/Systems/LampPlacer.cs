using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class LampPlacer : ISystem, ITickable
    {
        private IUnit _unit;
        private SystemService _systemService;
        private AstarService _astarService;
        private RoomService _roomService;

        public LampPlacer(IUnit unit, SystemService systemService, AstarService astarService,
            RoomService roomService)
        {
            _roomService = roomService;
            _unit = unit;
            _systemService = systemService;
            _astarService = astarService;
        }

        public void Tick()
        {
            if(_unit.LampPlacing == false)
                return;
            
            var inventory = _systemService.TryFindSystems<Inventory>()[0];
            
            if (inventory.Lamps.Value <= 0)
                return;
            
            var roomPosition = _astarService.CalculatePosition(_unit);
            if(_roomService.ReadyToPlaceLamp(roomPosition) == false)
                return;
            
            _roomService.PlaceLamp(roomPosition);
            inventory.Lamps.Decrease(1);
        }
    }
}