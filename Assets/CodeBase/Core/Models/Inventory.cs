using System;
using CodeBase.Core.Models.Systems;
using CodeBase.Infrastructure.Stats;

namespace CodeBase.Core.Models
{
    public class Inventory : ISingleSystem
    {
        public IntStat Bombs { get; private set; } = new(0);
        public IntStat RoomDestroyers { get; private set; } = new(0);
        public IntStat Lamps { get; private set; } = new(0);
    }
}