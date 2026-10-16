using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class Enrolment
{
    public int EnrolmentID { get; set; }
    public int ParticipantID { get; set; }
    public int CategoryID { get; set; }
    public DateTime EnrolmentDate { get; set; } = DateTime.Now;
    [Required] public string Status { get; set; } = "Confirmed";
    public User? Participant { get; set; }
    public Category? Category { get; set; }
    public RaceResult? Result { get; set; }
}
