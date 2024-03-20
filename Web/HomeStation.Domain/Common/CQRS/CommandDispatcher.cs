using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Domain.Common.CQRS;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    
    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken, string? clientId = null)
        where TCommand : class, ICommand
    {
        ICommandHandler<TCommand> handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.Handle(command, cancellationToken, clientId);
    }
}