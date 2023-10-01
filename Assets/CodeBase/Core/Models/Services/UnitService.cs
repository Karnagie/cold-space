using System.Linq;
using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Services.System;

namespace CodeBase.Core.Models.Services
{
    public class UnitService
    {
        private SystemService _systemService;

        public UnitService(SystemService systemService)
        {
            _systemService = systemService;
        }
        
        public bool TryFind(UnitTag tag, out IUnit unit)
        {
            var spiders = _systemService.TryFindSystems<IUnit>();
            
            var found = spiders.FirstOrDefault((unit => unit.Stats.Tag == tag));
            if (found != default)
            {
                unit = found;
                return true;
            }

            unit = null;
            return false;
        }
    }
}