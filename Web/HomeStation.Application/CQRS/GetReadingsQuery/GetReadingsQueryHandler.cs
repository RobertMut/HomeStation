using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;

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
        Device? device = await _unitOfWork.DeviceRepository.GetObjectBy(d =>
                d.Id == query.DeviceId,
            cancellationToken: cancellationToken,
            x => x.Climate.Where(c => GetReadingValueByDateType(query.DateType, c.Reading) > query.LastValuesFrom), 
            x => x.AirQuality.Where(a => GetReadingValueByDateType(query.DateType, a.Reading) > query.LastValuesFrom));

        if (device == null)
        {
            throw new Exception("Device not found"); //todo specific exception type
        }

        return GetReadings(device.Id, device.Climate, device.AirQuality);
    }

    private static IEnumerable<ReadingsWebModel> GetReadings(int device, IEnumerable<Climate> climateReadings, IEnumerable<Quality> airQualityReadings)
    {
        return from climate in climateReadings
            join air in airQualityReadings
                on climate.DeviceId equals air.DeviceId
            select
                new ReadingsWebModel
                {
                    DeviceId = device,
                    Temperature = climate.Temperature,
                    Humidity = climate.Humidity,
                    Pressure = climate.Pressure,
                    Pm1_0 = air.Pm1_0,
                    Pm2_5 = air.Pm2_5,
                    Pm10 = air.Pm10,
                    ReadDate = air.Reading.Date
                };
    }

    private static int GetReadingValueByDateType(DateType dateType, Reading reading)
    {
        return dateType switch
        {
            DateType.Day => reading.Day,
            DateType.Month => reading.Month,
            DateType.Week => reading.Week,
            _ => throw new NotSupportedException($"Date type {dateType} not supported")
        };
    }
}