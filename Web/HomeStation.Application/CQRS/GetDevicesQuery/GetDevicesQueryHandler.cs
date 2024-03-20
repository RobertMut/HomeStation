using System.Linq.Expressions;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.Entities;
using HomeStation.Domain.Common.Interfaces;

namespace HomeStation.Application.CQRS.GetDevicesQuery;

public class GetDevicesQueryHandler : IQueryHandler<GetDevicesQuery, IEnumerable<DeviceWebModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<DeviceWebModel>> Handle(GetDevicesQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Device> devices;
        
        using (_unitOfWork)
        {
            devices = _unitOfWork.DeviceRepository.GetAll();
        }
        
        return devices.Select(x => new DeviceWebModel()
        {
            Id = x.Id,
            Name = x.Name,
            IsKnown = x.IsKnown
        });
    }
}