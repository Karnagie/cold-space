using AStar;
using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Ui;
using CodeBase.UI;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private GameFactory _gameFactory;
        private WindowService _windowService;
        private UnitFactory _unitFactory;
        private RoomService _roomService;
        private EnemySpawner _enemySpawner;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            GameFactory gameFactory, WindowService windowService, UnitFactory unitFactory,
            RoomService roomService, EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
            _roomService = roomService;
            _windowService = windowService;
            _gameFactory = gameFactory;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
            _unitFactory = unitFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            var window = _windowService.CreateLevelMenu();
            window.Start.onClick.AddListener((() => _gameStateMachine.Enter<MenuState, string>("Menu")));
            
            _gameFactory.CreateInventory();
            
            CreateRooms();
            _unitFactory.CreatePlayer(new Vector3(0,0,0));
            _enemySpawner.StartSpawning();

            for (int i = 0; i < 10; i++)
            {
                if (_roomService.TryFindFreePositionForItem(out var transform))
                {
                    var random = new Random();
                    var nextType = random.Next(0, 3);
                    var type = ItemType.Bomb;
                    switch (nextType)
                    {
                        case 0:
                            type = ItemType.Bomb;
                            break;
                        case 1:
                            type = ItemType.RoomDestroyer;
                            break;
                        case 2:
                            type = ItemType.Lamp;
                            break;
                    }
                    _gameFactory.CreateLootSpawner(transform, type);
                }
            }
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void CreateRooms()
        {
            var roomMatrix = new []
            {
                new [] {6, 4, 4, 4, 8},
                new [] {1, 0, 0, 0, 2},
                new [] {5, 3, 3, 3, 7},
            };

            for (int y = 0; y < roomMatrix.Length; y++)
            {
                for (int x = 0; x < roomMatrix[y].Length; x++)
                {
                    var position = new Vector2(x * 6.25f - (roomMatrix[y].Length / 2 * 6.25f), y * 3);
                    switch (roomMatrix[y][x])
                    {
                        case 0:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Default,
                                    new Position(y, x),
                                    RoomMoveDisable.None);
                            break;
                        case 1:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Left,
                                    new Position(y, x),
                                    RoomMoveDisable.None);
                            break;
                        case 2:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Right,
                                    new Position(y, x),
                                    RoomMoveDisable.None);
                            break;
                        case 3:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Up,
                                    new Position(y, x),
                                    RoomMoveDisable.Up);
                            break;
                        case 4:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Down,
                                    new Position(y, x),
                                    RoomMoveDisable.Down);
                            break;
                        case 5:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Left,
                                    new Position(y, x),
                                    RoomMoveDisable.Up);
                            break;
                        case 6:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Left,
                                    new Position(y, x),
                                    RoomMoveDisable.Down);
                            break;
                        case 7:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Right,
                                    new Position(y, x),
                                    RoomMoveDisable.Up);
                            break;
                        case 8:
                            _gameFactory
                                .CreateRoom(
                                    position,
                                    RoomType.Right,
                                    new Position(y, x),
                                    RoomMoveDisable.Down);
                            break;
                    }
                }
            }
        }

        public class Factory : PlaceholderFactory<GameStateMachine, SceneLoader, LoadingCurtain, LoadLevelState>
        {
        }
    }

    public static class RandomExtensions
    {
        public static void Shuffle<T> (this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}