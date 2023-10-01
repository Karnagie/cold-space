using System;
using System.Collections;
using CodeBase.Core.Behaviours;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factories
{
    public class ViewFactory
    {
        private const string DefaultPlayerPath = "Characters/Player";
        private const string DefaultEnemyPath = "Characters/Enemy";
        
        private const string DefaultRoomPath = "Rooms/DefaultRoom";
        private const string LeftRoomPath = "Rooms/LeftRoom";
        private const string RightRoomPath = "Rooms/RightRoom";
        private const string UpRoomPath = "Rooms/UpRoom";
        private const string DownRoomPath = "Rooms/DownRoom";
        
        private const string WorldPath = "World";
        
        private const string InventoryPath = "UI/Inventory";

        private const string BombPath = "Items/Bomb";
        private const string RoomDestroyerPath = "Items/RoomDestroyer";
        private const string LampPath = "Items/Lamp";
        
        private const string TimerPath = "Visual/Timer";
        
        private IAssetProvider _assetProvider;
        private ICoroutineRunner _coroutineRunner;


        public ViewFactory(IAssetProvider assetProvider, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _assetProvider = assetProvider;
        }

        public UnitBehaviour Player(Vector3 position)
        {
            UnitBehaviour prefab = _assetProvider.Instantiate<UnitBehaviour>(DefaultPlayerPath);
            prefab.Transform.position = position;
            
            return prefab;
        }

        public UnitBehaviour Enemy(Vector3 position)
        {
            UnitBehaviour prefab = _assetProvider.Instantiate<UnitBehaviour>(DefaultEnemyPath);
            prefab.Transform.position = position;
            
            return prefab;
        }

        public WorldBehaviour World()
        {
            return _assetProvider.Instantiate<WorldBehaviour>(WorldPath);
        }

        public RoomBehaviour Room(Vector3 position, RoomType roomType)
        {
            RoomBehaviour room;
            switch (roomType)
            {
                case RoomType.Default:
                    room = _assetProvider.Instantiate<RoomBehaviour>(DefaultRoomPath);
                    break;
                case RoomType.Left:
                    room = _assetProvider.Instantiate<RoomBehaviour>(LeftRoomPath);
                    break;
                case RoomType.Right:
                    room = _assetProvider.Instantiate<RoomBehaviour>(RightRoomPath);
                    break;
                case RoomType.Up:
                    room = _assetProvider.Instantiate<RoomBehaviour>(UpRoomPath);
                    break;
                case RoomType.Down:
                    room = _assetProvider.Instantiate<RoomBehaviour>(DownRoomPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null);
            }
            
            room.Transform.position = position;

            return room;
        }

        public ItemBehaviour Item(Transform parent, ItemType type)
        {
            ItemBehaviour prefab;
            switch (type)
            {
                case ItemType.Bomb:
                    prefab = _assetProvider.Instantiate<ItemBehaviour>(BombPath);
                    break;
                case ItemType.RoomDestroyer:
                    prefab = _assetProvider.Instantiate<ItemBehaviour>(RoomDestroyerPath);
                    break;
                case ItemType.Lamp:
                    prefab = _assetProvider.Instantiate<ItemBehaviour>(LampPath);
                    break;
                default:
                    prefab = _assetProvider.Instantiate<ItemBehaviour>(BombPath);
                    break;
            }

            prefab.Transform.SetParent(parent);
            prefab.Transform.localPosition = Vector3.zero;

            return prefab;
        }

        public InventoryBehaviour Inventory()
        {
            return _assetProvider.Instantiate<InventoryBehaviour>(InventoryPath);
        }

        public void Timer(Transform roomCenter)
        {
            var timer =  _assetProvider.Instantiate<GameObject>(TimerPath);
            timer.transform.SetParent(roomCenter);
            timer.transform.localPosition = Vector3.zero;
            _coroutineRunner.StartCoroutine(Destroying(timer));
        }

        private IEnumerator Destroying(GameObject timer)
        {
            yield return new WaitForSeconds(5);
            Object.Destroy(timer);
        }
    }
}