using System.Linq.Expressions;
using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeStation.Application.CQRS.GetReadingsQuery;

public class GetReadingsQueryHandler : IQueryHandler<GetReadingsQuery, IEnumerable<ReadingsWebModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReadingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<ReadingsWebModel>> Handle(GetReadingsQuery query, CancellationToken cancellationToken)
    {
        DateTimeOffset timeSubtract = query.DateType switch
        {
            DateType.Day => DateTimeOffset.Now.AddDays(-query.LastValuesFrom),
            DateType.Month => DateTimeOffset.Now.AddMonths(-query.LastValuesFrom),
            DateType.Week => DateTimeOffset.Now.AddDays((-query.LastValuesFrom)*7),
        };
        
        using IUnitOfWork unitOfWork = _unitOfWork;
        Device? device = await unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == query.DeviceId,
            i => i
                .Include(x => x.Climate.Where(y => y.Reading.Date > timeSubtract))
                .Include(x => x.AirQuality.Where(y => y.Reading.Date > timeSubtract)), 
            cancellationToken);

        if (device == null)
        {
            throw new Exception("Device not found"); //todo specific exception type
        }

        return GetReadings(query, device.Climate, device.AirQuality);
    }

    private static IEnumerable<ReadingsWebModel> GetReadings(GetReadingsQuery query, IEnumerable<Climate>? climateReadings, IEnumerable<Quality>? airQualityReadings)
    {
        if (climateReadings == null || airQualityReadings == null)
        {
            return Enumerable.Empty<ReadingsWebModel>();
        }
        
        return from climate in climateReadings
            join air in airQualityReadings
                on climate.DeviceId equals air.DeviceId
            select
                new ReadingsWebModel
                {
                    DeviceId = query.DeviceId,
                    Temperature = climate.Temperature,
                    Humidity = climate.Humidity,
                    Pressure = climate.Pressure,
                    Pm1_0 = air.Pm1_0,
                    Pm2_5 = air.Pm2_5,
                    Pm10 = air.Pm10,
                    ReadDate = air.Reading.Date
                };
    }
}