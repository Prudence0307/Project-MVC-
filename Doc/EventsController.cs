using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers;

public class EventsController : Controller
{
    private readonly RaceDayContext _db;
    public EventsController(RaceDayContext db) => _db = db;

    public async Task<IActionResult> Index(string? type, string? location)
    {
        var query = _db.Events.Include(e => e.Organiser).Include(e => e.Categories).AsQueryable();
        if (!string.IsNullOrWhiteSpace(type)) query = query.Where(e => e.EventType == type);
        if (!string.IsNullOrWhiteSpace(location)) query = query.Where(e => e.Location.Contains(location));
        return View(await query.OrderBy(e => e.EventDate).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var ev = await _db.Events.Include(e => e.Organiser).Include(e => e.Categories)
            .ThenInclude(c => c.Route).FirstOrDefaultAsync(e => e.EventID == id);
        return ev == null ? NotFound() : View(ev);
    }

    [Authorize(Roles = "Organiser")]
    public IActionResult Create() => View(new RaceEvent { EventDate = DateTime.Today.AddDays(7) });

    [HttpPost, Authorize(Roles = "Organiser"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RaceEvent model)
    {
        ModelState.Remove(nameof(model.Organiser));
        if (!ModelState.IsValid) return View(model);
        model.OrganiserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _db.Events.Add(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = model.EventID });
    }

    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Edit(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        if (!Owns(ev)) return Forbid();
        return View(ev);
    }

    [HttpPost, Authorize(Roles = "Organiser"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RaceEvent model)
    {
        if (id != model.EventID) return BadRequest();
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        if (!Owns(ev)) return Forbid();
        ModelState.Remove(nameof(model.Organiser));
        if (!ModelState.IsValid) return View(model);
        ev.EventName = model.EventName; ev.EventDate = model.EventDate; ev.Location = model.Location;
        ev.Description = model.Description; ev.EventType = model.EventType;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, Authorize(Roles = "Organiser"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();
        if (!Owns(ev)) return Forbid();
        _db.Events.Remove(ev); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Owns(RaceEvent ev) => ev.OrganiserID.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier);
}
