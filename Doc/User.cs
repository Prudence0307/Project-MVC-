using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class User
{
    public int UserID { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Participant";
    [Phone, StringLength(20)] public string? PhoneNumber { get; set; }
    public DateTime DateRegistered { get; set; } = DateTime.Now;
    public ICollection<RaceEvent> OrganisedEvents { get; set; } = new List<RaceEvent>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
