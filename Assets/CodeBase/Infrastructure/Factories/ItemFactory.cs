using CodeBase.Core.Models;
using CodeBase.Core.Models.Components;
using CodeBase.Infrastructure.Services.System;

namespace CodeBase.Infrastructure.Factories
{
    public class ItemFactory
    {
        private SystemService _systemService;

        public ItemFactory(SystemService systemService)
        {
            _systemService = systemService;
        }
        
        public Item Create(ItemComponents components, ItemType type)
        {
            var inventory = _systemService.TryFindSystems<Inventory>()[0];
            return new Item(components, inventory, type);
        }
    }
}