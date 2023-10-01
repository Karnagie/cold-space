using CodeBase.Core.Models.Commands;
using CodeBase.Core.Models.Components;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Helpers;

namespace CodeBase.Core.Models
{
    public class Unit : IUnit, ICommandHandler<IUnitCommand>
    {
        public UnitStats Stats { get; }
        public SpiderComponents Components { get; }

        public Observable Killed { get; } = new();
        public bool Picking { get; set; }
        public bool BombPlacing { get; set; }
        public bool RoomDestroyerPlacing { get; set; }
        public bool LampPlacing { get; set; }

        public Unit(UnitStats stats, SpiderComponents components)
        {
            Stats = stats;
            Components = components;
        }

        public void Perform(IUnitCommand command)
        {
            command.Perform(this);
        }

        public void Kill()
        {
            Killed.Invoke();
        }
    }

    public interface IUnit : ISingleSystem
    {
        UnitStats Stats { get; }
        SpiderComponents Components { get; }

        Observable Killed { get; }
        bool Picking { get; set; }
        bool BombPlacing { get; set; }
        bool RoomDestroyerPlacing { get; set; }
        bool LampPlacing { get; set; }

        void Kill();
    }
}