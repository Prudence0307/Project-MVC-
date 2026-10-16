using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models;

public class RaceResult
{
    public int ResultID { get; set; }
    public int EnrolmentID { get; set; }
    public TimeSpan? FinishTime { get; set; }
    public int? Position { get; set; }
    [Required] public string Status { get; set; } = "Finished";
    public Enrolment? Enrolment { get; set; }
}
