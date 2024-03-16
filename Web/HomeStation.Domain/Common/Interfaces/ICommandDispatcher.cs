using System.Windows.Input;

namespace HomeStation.Domain.Common.Interfaces;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : class, ICommand;
}