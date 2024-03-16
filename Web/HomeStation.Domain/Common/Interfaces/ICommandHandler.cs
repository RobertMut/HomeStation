
namespace HomeStation.Domain.Common.Interfaces;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{ 
    Task Handle(TCommand command, CancellationToken cancellationToken);
}