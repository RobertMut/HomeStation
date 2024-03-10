namespace HomeStation.Domain.Common.Entities;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public IEnumerable<Climate> Climate { get; set; }
    public IEnumerable<Quality> AirQuality { get; set; }
}