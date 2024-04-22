using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Application.CQRS.ApproveRevokeDeviceCommand;

public class ApproveRevokeDeviceCommandHandler : ICommandHandler<ApproveRevokeDeviceCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveRevokeDeviceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ApproveRevokeDeviceCommand command, CancellationToken cancellationToken, string? identity = null)
    {
        using (_unitOfWork)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == command.Id && x.Name == command.Name, cancellationToken: cancellationToken);

            if (device == null)
            {
                throw new Exception("No device to approve.");
            }

            device.IsKnown = command.Operation == OperationType.Approve;
            
            _unitOfWork.DeviceRepository.UpdateAsync(device);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}