using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStation.Domain.Common.Entities;

public class Climate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int DeviceId { get; set; }

    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Pressure { get; set; }
    public Reading Reading { get; set; }
    
    public Device Device { get; set; }
}