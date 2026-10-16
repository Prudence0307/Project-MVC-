using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;

namespace RaceDayMVC.Controllers;
public class HomeController : Controller
{
    private readonly RaceDayContext _db;
    public HomeController(RaceDayContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.EventCount = await _db.Events.CountAsync();
        ViewBag.ParticipantCount = await _db.Users.CountAsync(u => u.Role == "Participant");
        ViewBag.EnrolmentCount = await _db.Enrolments.CountAsync(e => e.Status == "Confirmed");
        var events = await _db.Events.Include(e => e.Categories).Where(e => e.EventDate >= DateTime.Today)
            .OrderBy(e => e.EventDate).Take(6).ToListAsync();
        return View(events);
    }
}
