using CodeBase.Infrastructure.Helpers;
using CodeBase.Infrastructure.Stats;

namespace CodeBase.Core.Models.Components
{
    public class UnitStats
    {
        public IntStat Health;

        public UnitStats(int health, UnitTag tag)
        {
            Health = new IntStat(health);
            Tag = tag;
        }

        public UnitTag Tag;
    }
}