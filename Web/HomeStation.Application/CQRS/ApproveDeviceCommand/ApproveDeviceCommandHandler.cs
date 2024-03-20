using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Application.CQRS.ApproveDeviceCommand;

public class ApproveDeviceCommandHandler : ICommandHandler<ApproveDeviceCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveDeviceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ApproveDeviceCommand command, CancellationToken cancellationToken, string? identity = null)
    {
        using (_unitOfWork)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == command.Id && x.Name == command.Name, cancellationToken);

            if (device == null)
            {
                throw new Exception("No device to approve.");
            }

            device.IsKnown = true;
            
            _unitOfWork.DeviceRepository.UpdateAsync(device);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}