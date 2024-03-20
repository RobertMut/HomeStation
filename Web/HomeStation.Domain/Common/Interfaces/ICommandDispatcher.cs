using System.Windows.Input;

namespace HomeStation.Domain.Common.Interfaces;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken, string? cliendId = null)
        where TCommand : class, ICommand;
}