using CodeBase.Infrastructure.Services.System;
using Zenject;

namespace CodeBase.Infrastructure.Services.Ticking
{
    public class TickService : ITickable, IFixedTickable
    {
        private SystemService _systemService;

        public TickService(SystemService systemService)
        {
            _systemService = systemService;
        }
        
        public void Tick()
        {
            var tickables = _systemService.TryFindSystems<ITickable>();
            
            foreach (var tickable in tickables)
            {
                tickable.Tick();
            }
        }

        public void FixedTick()
        {
            var fixedTickables = _systemService.TryFindSystems<IFixedTickable>();
            
            foreach (var tickable in fixedTickables)
            {
                tickable.FixedTick();
            }
        }
    }
}