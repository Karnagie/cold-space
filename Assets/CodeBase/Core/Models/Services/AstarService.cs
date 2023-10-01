using AStar;
using CodeBase.Core.Behaviours;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Physics;
using CodeBase.Infrastructure.Services.System;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Core.Models.Services
{
    public class AstarService
    {
        private SystemService _systemService;
        private IPhysicsService _physicsService;

        public AstarService(SystemService systemService, IPhysicsService physicsService)
        {
            _systemService = systemService;
            _physicsService = physicsService;
        }

        public Position CalculatePosition(IUnit unit)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            var unitPosition = unit.Components.Transform.position;
            float minDistance = float.MaxValue;
            Room closestRoom = null;
            foreach (var room in rooms)
            {
                if (minDistance >= GetDistance(room, unitPosition))
                {
                    closestRoom = room;
                    minDistance = GetDistance(room, unitPosition);
                }
            }
            
            return closestRoom.Position;
        }

        private float GetDistance(Room room, Vector3 unit)
        {
            return Vector3.Distance(room.Components.Center.position, unit);
        }

        public Vector3 CalculateWorldPosition(Position cellPosition)
        {
            var rooms = _systemService.TryFindSystems<Room>();
            
            foreach (var room in rooms)
            {
                if (room.Position == cellPosition)
                {
                    return room.Components.Center.position;
                }
            }
            
            return Vector3.zero;
        }
    }
}