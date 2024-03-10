namespace HomeStation.Domain.Common.Entities;

public class Reading
{
    public int Month { get; set; }
    public int Week { get; set; }
    public int Day { get; set; }
    public DateTimeOffset Date { get; set; }
}