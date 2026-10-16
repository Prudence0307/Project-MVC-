using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers;

[Authorize]
public class EnrolmentsController : Controller
{
    private readonly RaceDayContext _db;
    public EnrolmentsController(RaceDayContext db) => _db = db;

    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Mine()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return View(await _db.Enrolments.Include(e => e.Category).ThenInclude(c => c!.Event)
            .Include(e => e.Result).Where(e => e.ParticipantID == userId).OrderByDescending(e => e.EnrolmentDate).ToListAsync());
    }

    [HttpPost, Authorize(Roles = "Participant"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int categoryId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var category = await _db.Categories.Include(c => c.Enrolments).FirstOrDefaultAsync(c => c.CategoryID == categoryId);
        if (category == null) return NotFound();
        if (category.Enrolments.Any(e => e.ParticipantID == userId)) { TempData["Error"] = "You are already enrolled in this category."; return RedirectToAction("Details", "Categories", new { id = categoryId }); }
        if (category.Enrolments.Count(e => e.Status == "Confirmed") >= category.MaxParticipants) { TempData["Error"] = "This category is full."; return RedirectToAction("Details", "Categories", new { id = categoryId }); }
        _db.Enrolments.Add(new Enrolment { ParticipantID = userId, CategoryID = categoryId, Status = "Confirmed" });
        await _db.SaveChangesAsync();
        TempData["Success"] = "You have successfully enrolled.";
        return RedirectToAction(nameof(Mine));
    }

    [HttpPost, Authorize(Roles = "Participant"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var enrolment = await _db.Enrolments.FindAsync(id);
        if (enrolment == null) return NotFound();
        if (enrolment.ParticipantID != userId) return Forbid();
        enrolment.Status = "Cancelled";
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Mine));
    }
}
