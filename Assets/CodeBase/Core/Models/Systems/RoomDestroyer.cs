using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class RoomDestroyer : ISystem, ITickable
    {
        private IUnit _unit;
        private SystemService _systemService;
        private AstarService _astarService;
        private RoomService _roomService;
        private ViewFactory _viewFactory;

        public RoomDestroyer(IUnit unit, SystemService systemService, AstarService astarService,
            RoomService roomService, ViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
            _roomService = roomService;
            _unit = unit;
            _systemService = systemService;
            _astarService = astarService;
        }

        public void Tick()
        {
            if(_unit.RoomDestroyerPlacing == false)
                return;
            
            var inventory = _systemService.TryFindSystems<Inventory>()[0];
            
            if (inventory.RoomDestroyers.Value <= 0)
                return;
            
            var roomPosition = _astarService.CalculatePosition(_unit);
            if(_roomService.ReadyToDestroyRoom(roomPosition) == false)
                return;
            
            var room = _roomService.StartDestroyingRoom(roomPosition);
            inventory.RoomDestroyers.Decrease(1);
            _viewFactory.Timer(room.Components.Center);
        }
    }
}