using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Application.CQRS.ApproveDeviceCommand;

public class ApproveDeviceCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; }
}