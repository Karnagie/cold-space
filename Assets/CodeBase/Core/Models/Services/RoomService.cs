using System.Collections;
using System.Collections.Generic;
using AStar;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace CodeBase.Core.Models.Services
{
    public class RoomService
    {
        private SystemService _systemService;

        private Dictionary<Room, List<Transform>> _roomSpawnPositions = new();
        private AstarService _astarService;

        public RoomService(SystemService systemService, AstarService astarService)
        {
            _astarService = astarService;
            _systemService = systemService;
        }
        
        public bool TryFindFreePositionForItem(out Transform transform)
        {
            transform = null;
            var rooms = _systemService.TryFindSystems<Room>();
            var random = new Random();
            random.Shuffle(rooms);
            foreach (var room in rooms)
            {
                if (HasFreePositions(room))
                {
                    var randomPosition = _roomSpawnPositions[room][0];
                    _roomSpawnPositions[room].Remove(randomPosition);
                    transform = randomPosition;
                    return true;
                }
            }

            return false;
        }
        
        public bool TryFindFreeRoom(out Transform transform)
        {
            transform = null;
            var rooms = _systemService.TryFindSystems<Room>();

            var playerFilter = new Filter<IUnit>((unit => unit.Stats.Tag == UnitTag.Player));
            var units = _systemService.TryFindSystems<IUnit>(playerFilter);
            if (units.Length == 0)
                return false;
            var player = units[0];
                
            var random = new Random();
            random.Shuffle(rooms);
            foreach (var room in rooms)
            {
                if (room.RoomDestroyed.Value == false && _astarService.CalculatePosition(player) != room.Position)
                {
                    transform = room.Components.Center;
                    return true;
                }
            }
            
            return false;
        }

        private bool HasFreePositions(Room room)
        {
            if (_roomSpawnPositions.ContainsKey(room) == false)
            {
                _roomSpawnPositions.Add(room, new List<Transform>(room.Components.SpawnPoints));
                return true;
            }
            if (_roomSpawnPositions[room].Count == 0)
                return false;
            
            return true;
        }

        public bool ReadyToExplodeWindow(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition && room.Type != RoomType.Default)
                {
                    return room.ReadyToExplodeWindow;
                }
            }
            return false;
        }

        public bool ReadyToDestroyRoom(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition)
                {
                    return room.ReadyToDestroyRoom;
                }
            }
            return false;
        }

        public Room StartExplodingWindow(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition && room.Type != RoomType.Default)
                {
                    room.ExplodeWindow();
                    return room;
                }
            }

            return null;
        }

        public void KillAllIn(Position roomPosition)
        {
            var units = _systemService.TryFindSystems<IUnit>();
            foreach (var unit in units)
            {
                if (_astarService.CalculatePosition(unit) == roomPosition)
                {
                    unit.Kill();
                }
            }
        }

        public Room StartDestroyingRoom(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition)
                {
                    room.RoomDestroy();
                    return room;
                }
            }

            return null;
        }

        public bool IsBlocked(int x, int y)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == new Position(y, x) && room.RoomDestroyed.Value)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ReadyToPlaceLamp(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition)
                {
                    return !room.IsLampOn.Value;
                }
            }
            return false;
        }

        public void PlaceLamp(Position roomPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            foreach (var room in rooms)
            {
                if (room.Position == roomPosition)
                {
                    room.PlaceLamp();
                }
            }
        }
    }

    public class EnemySpawner
    {
        private ICoroutineRunner _coroutineRunner;
        private UnitFactory _unitFactory;
        private RoomService _roomService;

        public EnemySpawner(ICoroutineRunner coroutineRunner, UnitFactory unitFactory,RoomService roomService)
        {
            _roomService = roomService;
            _unitFactory = unitFactory;
            _coroutineRunner = coroutineRunner;
        }
        
        public void StartSpawning()
        {
            _coroutineRunner.StartCoroutine(Spawning());
            
        }

        private IEnumerator Spawning()
        {
            for (int i = 0; i < 10; i++)
            {
                if (_roomService.TryFindFreeRoom(out var position) == false)
                    continue;
                
                _unitFactory.CreateEnemy(position.position);
                
                yield return new WaitForSeconds(20);
            }
        }
    }
}