using HomeStation.Application.Common.Enums;
using HomeStation.Domain.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeStation.Application.CQRS.GetReadingsQuery;

public class GetReadingsQuery
{
    public int DeviceId { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public DateType DateType { get; set; } = DateType.Day;

    public int LastValuesFrom { get; set; } = 3;
}