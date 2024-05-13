using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeStation.Application.CQRS.GetLatestReadingQuery;

public class GetLatestReadingQueryHandler : IQueryHandler<GetLatestReadingQuery, ReadingsWebModel>
{    
    
    private readonly IUnitOfWork _unitOfWork;

    public GetLatestReadingQueryHandler(IUnitOfWork unitOfWork) 
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ReadingsWebModel> Handle(GetLatestReadingQuery query, CancellationToken cancellationToken)
    {
        Climate? climate = await _unitOfWork.ClimateRepository.GetLastBy(x => x.DeviceId == query.DeviceId);
        Quality? quality = await _unitOfWork.QualityRepository.GetLastBy(x => x.DeviceId == query.DeviceId);
        
        if (climate == null || quality == null)
        {
            throw new Exception("No readings found");
        }

        return new ReadingsWebModel()
        {
            DeviceId = query.DeviceId,
            Humidity = climate.Humidity,
            Pressure = climate.Pressure,
            Temperature = climate.Temperature,
            Pm1_0 = quality.Pm1_0,
            Pm2_5 = quality.Pm2_5,
            Pm10 = quality.Pm10,
            ReadDate = quality.Reading.Date
        };
    }
}