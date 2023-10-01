using CodeBase.Core.Models.Services;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.Physics;
using UnityEngine;
using Zenject;

namespace CodeBase.Core.Models.Systems
{
    public class EnemyKiller : IDamager, ITickable
    {
        private readonly IUnit _model;
        private readonly IPhysicsService _physicsService;
        private readonly DeathService _deathService;

        public EnemyKiller(IUnit model, DeathService deathService, IPhysicsService physicsService)
        {
            _deathService = deathService;
            _physicsService = physicsService;
            _model = model;
        }

        public void TryDamage()
        {
            var filterTag = new Filter<IUnit>((unit) => unit.Stats.Tag == UnitTag.Player);
            var filterCollision = new Filter<IUnit>((unit) =>
                _physicsService.HasCollision(_model.Components.Collider, unit.Components.Collider));
            
            _deathService.TryKill(filterTag, filterCollision);
        }

        public void Tick()
        {
            TryDamage();
        }
    }
}