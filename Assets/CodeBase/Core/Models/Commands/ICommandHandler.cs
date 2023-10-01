namespace CodeBase.Core.Models.Commands
{
    public interface ICommandHandler<TCommand>
    {
        void Perform(TCommand command);
    }
}