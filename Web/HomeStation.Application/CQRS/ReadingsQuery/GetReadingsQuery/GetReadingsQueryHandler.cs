using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeStation.Application.CQRS.ReadingsQuery.GetReadingsQuery;

public class GetReadingsQueryHandler : IQueryHandler<GetReadingsQuery, IEnumerable<ReadingsWebModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReadingsQueryHandler(IUnitOfWork unitOfWork) 
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ReadingsWebModel>?> Handle(GetReadingsQuery query, CancellationToken cancellationToken)
    {
        Device? device = await GetData(query, cancellationToken);
        
        if (device == null)
        {
            throw new Exception("Device not found"); //todo specific exception type
        }
        
        IEnumerable<ReadingsWebModel>? readings = GetReadings(query, device.Climate, device.AirQuality);

        return LimitDataCount(readings, query.DetailLevel);
    }
    private async Task<Device?> GetData(GetReadingsQuery query, CancellationToken cancellationToken)
    {
        using IUnitOfWork unitOfWork = _unitOfWork;
        Device? device;
        
        switch (query.ReadingType)
        {
            case ReadingType.Quality:
                device = await unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == query.DeviceId,
                    i => i
                        .Include(x => x.AirQuality.Where(y => 
                            query.StartDate < y.Reading.Date &&
                            y.Reading.Date < query.EndDate)), 
                    cancellationToken);
                break;
            case ReadingType.Climate:
                device = await unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == query.DeviceId,
                    i => i
                        .Include(x => x.Climate.Where(y => 
                            query.StartDate < y.Reading.Date &&
                            y.Reading.Date < query.EndDate)), 
                    cancellationToken);
                break;
            case ReadingType.Complete:
                device = await unitOfWork.DeviceRepository.GetObjectBy(x => x.Id == query.DeviceId,
                    i => i
                        .Include(x => x.Climate.Where(y => 
                            query.StartDate < y.Reading.Date &&
                            y.Reading.Date < query.EndDate))
                        .Include(x => x.AirQuality.Where(y => 
                            query.StartDate < y.Reading.Date &&
                            y.Reading.Date < query.EndDate)), 
                    cancellationToken);
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        return device;
    }
    
    private static IEnumerable<ReadingsWebModel>? GetReadings(GetReadingsQuery query, IEnumerable<Climate>? climateReadings, IEnumerable<Quality>? airQualityReadings)
    {
        if (climateReadings == null)
        {
            return airQualityReadings.Select(x => new ReadingsWebModel
            {
                DeviceId = query.DeviceId,
                Pm1_0 = x?.Pm1_0,
                Pm2_5 = x?.Pm2_5,
                Pm10 = x?.Pm10,
                ReadDate = x.Reading.Date
            });
        }

        if (airQualityReadings == null)
        {
            return climateReadings.Select(x => new ReadingsWebModel
            {
                DeviceId = query.DeviceId,
                Temperature = x?.Temperature,
                Humidity = x?.Humidity,
                Pressure = x?.Pressure,
                ReadDate = x.Reading.Date
            });
        }
        
        return from climate in climateReadings
            join air in airQualityReadings
                on climate?.DeviceId equals air?.DeviceId
            select
                new ReadingsWebModel
                {
                    DeviceId = query.DeviceId,
                    Temperature = climate?.Temperature,
                    Humidity = climate?.Humidity,
                    Pressure = climate?.Pressure,
                    Pm1_0 = air?.Pm1_0,
                    Pm2_5 = air?.Pm2_5,
                    Pm10 = air?.Pm10,
                    ReadDate = air.Reading.Date
                };
    }
    
    private IEnumerable<ReadingsWebModel>? LimitDataCount(IEnumerable<ReadingsWebModel>? readings, DetailLevel detailLevel)
    {
        if (readings == null || !readings.Any())
        {
            return Enumerable.Empty<ReadingsWebModel>();
        }
        
        int? limit = detailLevel switch
        {
            DetailLevel.Normal => 1000,
            DetailLevel.Less => 500,
            DetailLevel.Detailed => null
        };

        if (!limit.HasValue || readings.Count() <= limit.Value)
        {
            return readings;
        }

        int skip = readings.Count() / limit.Value;

        return GetNth(readings.ToList(), skip);
    }

    private static IEnumerable<ReadingsWebModel>? GetNth(List<ReadingsWebModel> readings, int step)
    {
        for (int i = 0; i < readings.Count(); i += step)
        {
            yield return readings[i];
        }
    }
}