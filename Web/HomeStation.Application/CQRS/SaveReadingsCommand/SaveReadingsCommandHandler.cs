using System.Globalization;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.Data.SqlClient;

namespace HomeStation.Application.CQRS.SaveReadingsCommand;

public class SaveReadingsCommandHandler : ICommandHandler<SaveReadingsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveReadingsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(SaveReadingsCommand command, CancellationToken cancellationToken, string? clientId = null)
    {     
        using (_unitOfWork)
        {
            Device? device = await _unitOfWork.DeviceRepository.GetObjectBy(x => x.Name == clientId, cancellationToken: cancellationToken);
            await CheckDevice(device, clientId, cancellationToken);

            (Climate, Quality) entities = ConstructEntities(command, device.Id);
            
            await _unitOfWork.ClimateRepository.InsertAsync(entities.Item1, cancellationToken);
            await _unitOfWork.QualityRepository.InsertAsync(entities.Item2, cancellationToken);

            await _unitOfWork.Save(cancellationToken);
        }
    }

    private async Task CheckDevice(Device? device, string? deviceId, CancellationToken cancellationToken)
    {
        if (device == null)
        {
            device = new Device
            {
                Name = deviceId,
                IsKnown = false
            };
                
            await _unitOfWork.DeviceRepository.InsertAsync(device, cancellationToken);
            await _unitOfWork.Save(cancellationToken);
        }

        if (!device.IsKnown)
        {
            throw new Exception("Device is not known.");
        }
    }

    private static (Climate, Quality) ConstructEntities(SaveReadingsCommand command, int deviceId)
    {
        Reading reading = GetCurrentReading();
        
        var climate = new Climate
        {
            DeviceId = deviceId,
            Temperature = command.Temperature,
            Humidity = command.Humidity,
            Pressure = command.Pressure,
            Reading = reading
        };

        var quality = new Quality()
        {
            DeviceId = deviceId,
            Pm1_0 = command.Pm1_0,
            Pm2_5 = command.Pm2_5,
            Pm10 = command.Pm10,
            Reading = reading
        };

        return (climate, quality);  
    }

    private static Reading GetCurrentReading() =>
        new()
        {
            Date = DateTimeOffset.Now,
        };
}