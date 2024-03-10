using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStation.Domain.Common.Entities;

public class Quality
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int DeviceId { get; set; }
    
    public int Pm2_5 { get; set; }
    public int Pm10 { get; set; }
    public int Pm1_0 { get; set; }
    public Reading Reading { get; set; }
    
    public Device Device { get; set; }
}