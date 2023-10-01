using System;
using CodeBase.Core.Models.Components;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Helpers;

namespace CodeBase.Core.Models
{
    public class Item : ISingleSystem
    {
        public readonly ItemComponents Components;
        private readonly Inventory _inventory;
        private readonly ItemType _type;

        public readonly Observable Picked = new ();

        public Item(ItemComponents components, Inventory inventory, ItemType type)
        {
            Components = components;
            _inventory = inventory;
            _type = type;
        }

        public void TryPick()
        {
            Picked.Invoke();
            switch (_type)
            {
                case ItemType.Bomb:
                    _inventory.Bombs.Increase(1);
                    break;
                case ItemType.RoomDestroyer:
                    _inventory.RoomDestroyers.Increase(1);
                    break;
                case ItemType.Lamp:
                    _inventory.Lamps.Increase(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}