using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly RaceDayContext _db;
    public CategoriesController(RaceDayContext db) => _db = db;

    public async Task<IActionResult> Details(int id)
    {
        var category = await _db.Categories.Include(c => c.Event).Include(c => c.Route)
            .Include(c => c.Enrolments).ThenInclude(e => e.Result)
            .Include(c => c.Enrolments).ThenInclude(e => e.Participant)
            .FirstOrDefaultAsync(c => c.CategoryID == id);
        return category == null ? NotFound() : View(category);
    }

    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Create(int eventId)
    {
        var ev = await _db.Events.FindAsync(eventId);
        if (ev == null) return NotFound();
        if (ev.OrganiserID.ToString() != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value) return Forbid();
        ViewBag.EventName = ev.EventName;
        return View(new Category { EventID = eventId, MaxParticipants = 500 });
    }

    [HttpPost, Authorize(Roles = "Organiser"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        var ev = await _db.Events.FindAsync(model.EventID);
        if (ev == null) return NotFound();
        if (ev.OrganiserID.ToString() != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value) return Forbid();
        ModelState.Remove(nameof(model.Event));
        if (!ModelState.IsValid) { ViewBag.EventName = ev.EventName; return View(model); }
        _db.Categories.Add(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = model.CategoryID });
    }
}
