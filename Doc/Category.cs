using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class Category
{
    public int CategoryID { get; set; }
    public int EventID { get; set; }
    [Required, StringLength(50)] public string CategoryName { get; set; } = "";
    [Range(0.1, 999.99)] public decimal Distance { get; set; }
    [Range(0, 999999)] public decimal EntryFee { get; set; }
    [Range(1, 100000)] public int MaxParticipants { get; set; } = 500;
    public RaceEvent? Event { get; set; }
    public RaceRoute? Route { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
