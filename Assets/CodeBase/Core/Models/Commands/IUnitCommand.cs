namespace CodeBase.Core.Models.Commands
{
    public interface IUnitCommand
    {
        void Perform(IUnit unit);
    }
}