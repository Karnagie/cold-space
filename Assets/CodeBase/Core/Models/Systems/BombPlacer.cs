using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class BombPlacer : ISystem, ITickable
    {
        private IUnit _unit;
        private SystemService _systemService;
        private AstarService _astarService;
        private RoomService _roomService;
        private ViewFactory _viewFactory;

        public BombPlacer(IUnit unit, SystemService systemService, AstarService astarService,
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
            if(_unit.BombPlacing == false)
                return;
            
            var inventory = _systemService.TryFindSystems<Inventory>()[0];
            
            if (inventory.Bombs.Value <= 0)
                return;
            
            var roomPosition = _astarService.CalculatePosition(_unit);
            if(_roomService.ReadyToExplodeWindow(roomPosition) == false)
                return;
            
            var room = _roomService.StartExplodingWindow(roomPosition);
            inventory.Bombs.Decrease(1);
            _viewFactory.Timer(room.Components.Center);
        }
    }
}