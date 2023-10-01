using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Physics;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class Pickable : ISystem, ITickable
    {
        private readonly Item _model;
        private UnitService _unitService;
        private IPhysicsService _physicsService;

        public Pickable(Item model, UnitService unitService, IPhysicsService physicsService)
        {
            _physicsService = physicsService;
            _unitService = unitService;
            _model = model;
        }

        public void Tick()
        {
            if (_unitService.TryFind(UnitTag.Player, out IUnit player))
            {
                if (_physicsService.HasCollision(_model.Components.Collider, player.Components.Collider) == false)
                    return;

                if (player.Picking)
                {
                    _model.TryPick();
                }
            }
        }
    }
}