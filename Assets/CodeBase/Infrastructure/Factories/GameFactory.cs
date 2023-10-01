using AStar;
using CodeBase.Core.Models;
using CodeBase.Core.Models.Components;
using CodeBase.Core.Models.Services;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.System;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class GameFactory
    {
        private ViewFactory _viewFactory;
        private SystemService _systemService;
        private ServiceSystemFactory _serviceSystemFactory;
        private BinderFactory _binderFactory;
        private ItemFactory _itemFactory;
        private ICoroutineRunner _coroutineRunner;
        private RoomService _roomService;

        public GameFactory(ViewFactory viewFactory, SystemService systemService,
            ServiceSystemFactory serviceSystemFactory, BinderFactory binderFactory, 
            ItemFactory itemFactory, ICoroutineRunner coroutineRunner, RoomService roomService)
        {
            _roomService = roomService;
            _coroutineRunner = coroutineRunner;
            _serviceSystemFactory = serviceSystemFactory;
            _binderFactory = binderFactory;
            _itemFactory = itemFactory;
            _systemService = systemService;
            _viewFactory = viewFactory;
        }

        public void CreateRoom(Vector2 position, RoomType type, Position astarPosition, RoomMoveDisable roomMoveDisable)
        {
            var behaviour = _viewFactory.Room(position, type);
            var binder = _binderFactory.Create();
            var linker = new SystemLinker();

            var components = new RoomComponents(behaviour.DownCollider, behaviour.Transform, behaviour.Center,
                behaviour.UpCollider, behaviour.SpawnPoints, behaviour.Lamp);

            Room model;
            model = new Room(components, astarPosition, _coroutineRunner, type);
            
            binder.Bind(model.WindowDestroyed, _ => behaviour.DestroyWindow());
            binder.Bind(model.WindowDestroyed, _ => _roomService.KillAllIn(model.Position));
            
            binder.Bind(model.RoomDestroyed, _ => behaviour.RoomDestroy());
            binder.Bind(model.RoomDestroyed, _ => _roomService.KillAllIn(model.Position));

            var physicBody = 
                _serviceSystemFactory.RoomPhysicsBody(behaviour.UpCollider, behaviour.DownCollider, roomMoveDisable);
            linker.Add(physicBody);
            linker.Add(model);
            
            binder.LinkHolder(_systemService, linker);
        }

        public void CreateLootSpawner(Transform parent, ItemType type)
        {
            var behaviour = _viewFactory.Item(parent, type);
            var binder = _binderFactory.Create(); 
            var linker = new SystemLinker();

            var components = new ItemComponents(behaviour.Collider);
            var model = _itemFactory.Create(components, type);
            
            var physicBody = 
                _serviceSystemFactory.DefaultBody(behaviour.Rigidbody, behaviour.Collider);
            var pickable = _serviceSystemFactory.Pickable(model);
            
            
            linker.Add(physicBody);
            linker.Add(model);
            linker.Add(pickable);
            
            binder.LinkHolder(_systemService, linker);
            
            binder.LinkEvent(model.Picked, binder.Dispose);
            binder.LinkEvent(model.Picked, () => Object.Destroy(behaviour.gameObject));
        }

        public void CreateInventory()
        {
            var behaviour = _viewFactory.Inventory();
            var binder = _binderFactory.Create(); 
            var linker = new SystemLinker();
            
            var model = new Inventory();
            linker.Add(model);
            
            binder.LinkHolder(_systemService, linker);
            
            binder.Bind(model.Bombs, (bombs => behaviour.BombsText.text = $"bombs: {bombs}"));
            binder.Bind(model.RoomDestroyers, 
                (destroyers => behaviour.RoomDestroyersText.text = $"room destroyers: {destroyers}"));
            binder.Bind(model.Lamps, (lamps => behaviour.LampsText.text = $"lamps: {lamps}"));
        }
    }

    public enum RoomType
    {
        Default,
        Left,
        Right,
        Up,
        Down,
    }
    
    public enum ItemType
    {
        Bomb,
        RoomDestroyer,
        Lamp,
    }

    public enum RoomMoveDisable
    {
        None,
        Up,
        Down,
    }
}