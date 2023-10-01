using CodeBase.Infrastructure.Services.Binding;
using CodeBase.Infrastructure.Services.System;
using Zenject;

namespace CodeBase.Core.Models.Services
{
    public class DeathService : ITickable
    {
        private SystemService _systemService;

        public DeathService(SystemService systemService)
        {
            _systemService = systemService;
        }
        
        public void Tick()
        {
            var models = _systemService.TryFindSystems<IUnit>();
            foreach (var model in models)
            {
                if (model.Stats.Health.Value <= 0)
                {
                    model.Kill();
                }
            }
        }

        public void TryKill(params IFilter[] filters)
        {
            var units = _systemService.TryFindSystems<IUnit>(filters);
            foreach (var unit in units)
            {
                unit.Kill();
            }
        }
    }
}