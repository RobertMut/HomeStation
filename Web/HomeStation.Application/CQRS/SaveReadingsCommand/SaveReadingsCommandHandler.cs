using System.Globalization;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Application.CQRS.SaveReadingsCommand;

public class SaveReadingsCommandHandler : ICommandHandler<SaveReadingsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SaveReadingsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(SaveReadingsCommand command, CancellationToken cancellationToken)
    {
        (Climate, Quality) entities = ConstructEntities(command);
        
        using (_unitOfWork)
        {
            await _unitOfWork.ClimateRepository.InsertAsync(entities.Item1, cancellationToken);
            await _unitOfWork.QualityRepository.InsertAsync(entities.Item2, cancellationToken);

            await _unitOfWork.Save(cancellationToken);
        }
    }

    private static (Climate, Quality) ConstructEntities(SaveReadingsCommand command)
    {
        Reading reading = GetCurrentReading();
        
        var climate = new Climate
        {
            DeviceId = command.DeviceId,
            Temperature = command.Temperature,
            Humidity = command.Humidity,
            Pressure = command.Pressure,
            Reading = reading
        };

        var quality = new Quality()
        {
            DeviceId = command.DeviceId,
            Pm1_0 = command.Pm1_0,
            Pm2_5 = command.Pm2_5,
            Pm10 = command.Pm10,
            Reading = reading
        };

        return (climate, quality);  
    }

    private static Reading GetCurrentReading()
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

        return new Reading()
        {
            Date = dateTimeOffset,
            Day = dateTimeOffset.Day,
            Month = dateTimeOffset.Month,
            Week = ISOWeek.GetWeekOfYear(dateTimeOffset.DateTime)
        };
    }
}