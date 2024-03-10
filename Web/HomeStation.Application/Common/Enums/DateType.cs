using System.ComponentModel.DataAnnotations;

namespace HomeStation.Application.Common.Enums;

public enum DateType
{
    [Display(Name = "Day")]
    Day,
    
    [Display(Name = "Month")]
    Month,
    
    [Display(Name = "Week")]
    Week,
}