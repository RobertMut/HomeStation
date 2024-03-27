using HomeStation.Application.CQRS.ApproveDeviceCommand;
using HomeStation.Application.CQRS.GetDevicesQuery;
using HomeStation.Application.CQRS.GetReadingsQuery;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeStation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public DeviceController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public async Task GetDevices()
        {
            await _queryDispatcher.Dispatch<GetDevicesQuery, IEnumerable<DeviceWebModel>>(new GetDevicesQuery(),
                new CancellationToken());
        }

        [HttpPost("approve")]
        public async Task Approve([FromBody] ApproveDeviceCommand command)
        {
            await _commandDispatcher.Dispatch(command, new CancellationToken());
        }
    }
}
