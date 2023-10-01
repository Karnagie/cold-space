using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.Physics;
using UnityEngine;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class DownMover : IFixedTickable, ISystem
    {
        private IInputService _inputService;
        private IUnit _model;
        private IPhysicsService _physicsService;

        public DownMover(IInputService inputService, IUnit model, IPhysicsService physicsService)
        {
            _physicsService = physicsService;
            _model = model;
            _inputService = inputService;
        }

        public void FixedTick()
        {
            TryEnableCollisionForRooms();
            TryDisableCollisionForRooms();
        }

        private void TryDisableCollisionForRooms()
        {
            var filterCollision = new Filter<Room>((room) =>
                _physicsService.HasCollision(_model.Components.Collider, room.Components.DownCollider) ||
                _physicsService.HasCollision(_model.Components.Collider, room.Components.UpCollider));
            var floors = _physicsService.All(filterCollision);

            if (_inputService.Moving().y < 0)
            {
                foreach (var floor in floors)
                {
                    floor.IgnoreCollisionWith(_model.Components.Collider, true);
                }
            }
        }
        
        private void TryEnableCollisionForRooms()
        {
            var filterCollision = new Filter<Room>((room) =>
                _physicsService.HasCollision(_model.Components.Collider, room.Components.DownCollider) == false ||
                _physicsService.HasCollision(_model.Components.Collider, room.Components.UpCollider) == false);
            var floors = _physicsService.All(filterCollision);

            if (_inputService.Moving().y >= 0)
            {
                foreach (var floor in floors)
                {
                    floor.IgnoreCollisionWith(_model.Components.Collider, false);
                }
            }
        }
    }
}