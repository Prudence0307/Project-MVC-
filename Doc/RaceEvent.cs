using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class RaceEvent
{
    public int EventID { get; set; }
    public int OrganiserID { get; set; }
    [Required, StringLength(150)] public string EventName { get; set; } = "";
    [DataType(DataType.Date)] public DateTime EventDate { get; set; }
    [Required, StringLength(150)] public string Location { get; set; } = "";
    public string? Description { get; set; }
    [Required] public string EventType { get; set; } = "Running";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public User? Organiser { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
