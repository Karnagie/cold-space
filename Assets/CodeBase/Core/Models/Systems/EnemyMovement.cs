using AStar;
using AStar.Options;
using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class EnemyMovement : IFixedTickable, ISystem
    {
        private IUnit _model;
        private UnitService _unitHolder;
        private AstarService _astarService;
        private RoomService _roomService;

        public EnemyMovement(IUnit model, UnitService unitHolder, AstarService astarService, RoomService roomService)
        {
            _roomService = roomService;
            _astarService = astarService;
            _unitHolder = unitHolder;
            _model = model;
        }

        public void FixedTick()
        {
            if (_unitHolder.TryFind(UnitTag.Player, out IUnit player))
            {
                var playerPosition = _astarService.CalculatePosition(player);
                var modelPosition = _astarService.CalculatePosition(_model);

                if (playerPosition == modelPosition)
                {
                    var delta = 
                        (player.Components.Transform.position- _model.Components.Transform.position).normalized;
                    _model.Components.Transform.Translate(delta*Time.deltaTime);
                    return;
                }
                
                
                var worldGrid = new WorldGrid(3, 5);
                SetupGrid(worldGrid, playerPosition, modelPosition, _roomService);
                
                
                var pathfinderOptions = new PathFinderOptions { 
                    PunishChangeDirection = true,
                    UseDiagonals = false, 
                };
                
                var pathfinder = new PathFinder(worldGrid, pathfinderOptions);
                var nextPositions = pathfinder.FindPath(modelPosition, playerPosition);

                if (nextPositions.Length >= 2)
                {
                    var roomPosition = _astarService.CalculateWorldPosition(nextPositions[1]);
                    var delta = (roomPosition - _model.Components.Transform.position).normalized;
                    _model.Components.Transform.Translate(delta*Time.deltaTime);
                }
            }
        }

        private void SetupGrid(WorldGrid worldGrid, Position playerPosition, Position modelPosition, RoomService roomService)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (roomService.IsBlocked(x, y))
                    {
                        worldGrid[y,x] = 0;
                    }
                    else
                    {
                        worldGrid[y,x] = 1;
                    }
                }
            }

            worldGrid[playerPosition.Row, playerPosition.Column] = 1;
            worldGrid[modelPosition.Row, modelPosition.Column] = 1;
        }
    }
}