using CodeBase.Core.Models;
using CodeBase.Core.Models.Services;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.Physics;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class ServiceSystemFactory
    {
        private IInputService _inputService;
        private UnitService _unitService;
        private IPhysicsService _physicsService;
        private DeathService _deathService;
        private AstarService _astarService;
        private SystemService _systemService;
        private RoomService _roomService;
        private ViewFactory _viewFactory;//todo remove

        public ServiceSystemFactory(IInputService inputService, UnitService unitService, 
            IPhysicsService physicsService, DeathService deathService, AstarService astarService,
            SystemService systemService, RoomService roomService, ViewFactory viewFactory)
        {
            _systemService = systemService;
            _roomService = roomService;
            _viewFactory = viewFactory;
            _astarService = astarService;
            _unitService = unitService;
            _physicsService = physicsService;
            _inputService = inputService;
            _deathService = deathService;
        }

        public ISystem PlayerMovement(IUnit model)
        {
            var movement = new PlayerMovement(_inputService, model);
            return movement;
        }

        public ISystem UnitDownMover(IUnit model)
        {
            var movement = new DownMover(_inputService, model, _physicsService);
            return movement;
        }
        
        public ISystem EnemyMovement(IUnit model)
        {
            var movement = new EnemyMovement(model, _unitService, _astarService, _roomService);
            return movement;
        }

        public ISystem EnemyKiller(IUnit model)
        {
            var killer = new EnemyKiller(model, _deathService, _physicsService);
            return killer;
        }

        public ISystem DefaultBody(IUnit model)
        {
            return new DefaultBody(model.Components.Rigidbody, model.Components.Collider);
        }

        public ISystem DefaultBody(Rigidbody2D rigidbody, Collider2D collider)
        {
            return new DefaultBody(rigidbody, collider);
        }

        public ISystem RoomPhysicsBody(Collider2D upCollider, Collider2D downCollider, RoomMoveDisable roomMoveDisable)
        {
            return new RoomPhysicBody(upCollider, downCollider, roomMoveDisable);
        }

        public ISystem Pickable(Item model)
        {
            return new Pickable(model, _unitService, _physicsService);
        }

        public ISystem BombPlacer(IUnit model)
        {
            return new BombPlacer(model, _systemService, _astarService, _roomService, _viewFactory);
        }
        
        public ISystem RoomDestroyer(IUnit model)
        {
            return new RoomDestroyer(model, _systemService, _astarService, _roomService, _viewFactory);
        }

        public ISystem LampPlacer(IUnit model)
        {
            return new LampPlacer(model, _systemService, _astarService, _roomService);
        }
    }
}