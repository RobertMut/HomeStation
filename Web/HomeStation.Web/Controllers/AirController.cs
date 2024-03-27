using HomeStation.Application.CQRS.GetReadingsQuery;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeStation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public AirController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet(template: "{DeviceId:required}/{DateType:alpha}/{LastValuesFrom:int}", 
            Name = "GetReadings")]
        public async Task<IEnumerable<ReadingsWebModel>> Get([FromRoute] GetReadingsQuery readingsQuery)
        {
            return await _queryDispatcher.Dispatch<GetReadingsQuery, IEnumerable<ReadingsWebModel>>(readingsQuery,
                new CancellationToken());
        }
    }
}
