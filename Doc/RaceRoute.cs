using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class RaceRoute
{
    public int RouteID { get; set; }
    public int CategoryID { get; set; }
    [StringLength(100)] public string? RouteName { get; set; }
    [Required, StringLength(150)] public string StartPoint { get; set; } = "";
    [Required, StringLength(150)] public string EndPoint { get; set; } = "";
    public int? ElevationGain { get; set; }
    [Url, StringLength(255)] public string? MapURL { get; set; }
    public Category? Category { get; set; }
}
