namespace CodeBase.Core.Models.Commands
{
    public class DamageCommand : IUnitCommand
    {
        public void Perform(IUnit unit)
        {
            unit.Stats.Health.Decrease(10);
        }
    }
}