using CodeBase.Core.Behaviours;
using CodeBase.Core.Models.Components;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.System;
using UnityEngine;
using IUnit = CodeBase.Core.Models.IUnit;
using Object = UnityEngine.Object;
using Unit = CodeBase.Core.Models.Unit;

namespace CodeBase.Infrastructure.Factories
{
    public class UnitFactory
    {
        private readonly ViewFactory _viewFactory;
        private readonly ServiceSystemFactory _serviceSystemFactory;
        private readonly SystemService _systemService;
        private BinderFactory _binderFactory;
        private IInputService _inputService;

        public UnitFactory(ViewFactory viewFactory,
            ServiceSystemFactory serviceSystemFactory,
            SystemService systemService,
            BinderFactory binderFactory,
            IInputService inputService)
        {
            _inputService = inputService;
            _binderFactory = binderFactory;
            _systemService = systemService;
            _serviceSystemFactory = serviceSystemFactory;
            _viewFactory = viewFactory;
        }

        public void CreatePlayer(Vector3 position)
        {
            var behaviour = _viewFactory.Player(position);
            
            var stats = new UnitStats(100, UnitTag.Player);
            var components = new SpiderComponents(behaviour.Transform, behaviour.Collider, behaviour.Rigidbody);
            var model = new Unit(stats, components);
            var binder = _binderFactory.Create();
            var linker = new SystemLinker();
            
            LinkPlayerSystems(model, binder, linker);
            BindSpider(model, binder, behaviour, linker);
        }
        
        public void CreateEnemy(Vector3 position)
        {
            var behaviour = _viewFactory.Enemy(position);
            
            var stats = new UnitStats(50, UnitTag.Enemy);
            var components = new SpiderComponents(behaviour.Transform, behaviour.Collider, behaviour.Rigidbody);
            var model = new Unit(stats, components);
            var binder = _binderFactory.Create();
            var linker = new SystemLinker();
            
            LinkEnemySystems(model, linker);
            BindSpider(model, binder, behaviour, linker);
        }

        private void LinkPlayerSystems(IUnit model, Binder binder, SystemLinker linker)
        {
            var playerMovement = _serviceSystemFactory.PlayerMovement(model);
            var verticalMover = _serviceSystemFactory.UnitDownMover(model);
            var bombPlacer = _serviceSystemFactory.BombPlacer(model);
            var roomDestroyer = _serviceSystemFactory.RoomDestroyer(model);
            var lampPlacer = _serviceSystemFactory.LampPlacer(model);
            
            linker.Add(playerMovement);
            linker.Add(verticalMover);
            linker.Add(model);
            linker.Add(roomDestroyer);
            linker.Add(bombPlacer);
            linker.Add(lampPlacer);
        }
        
        private void LinkEnemySystems(IUnit model, SystemLinker linker)
        {
            var enemyMovement = _serviceSystemFactory.EnemyMovement(model);
            var physicBody = _serviceSystemFactory.DefaultBody(model);
            var killer = _serviceSystemFactory.EnemyKiller(model);
            
            linker.Add(enemyMovement);
            linker.Add(model);
            linker.Add(physicBody);
            linker.Add(killer);
        }

        private void BindSpider(IUnit model, Binder binder, UnitBehaviour behaviour, SystemLinker linker)
        {
            binder.LinkHolder(_systemService, linker);

            BindDisposing(model, binder, behaviour);
        }

        private static void BindDisposing(IUnit model, Binder binder, UnitBehaviour behaviour)
        {
            binder.LinkEvent(model.Killed, binder.Dispose);
            binder.LinkEvent(model.Killed, (() => Object.Destroy(behaviour.gameObject)));
        }
    }
}