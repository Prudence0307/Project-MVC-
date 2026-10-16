using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayMVC.Data;

namespace RaceDayMVC.Controllers;
public class ResultsController : Controller
{
    private readonly RaceDayContext _db;
    public ResultsController(RaceDayContext db) => _db = db;

    public async Task<IActionResult> Leaderboard(int categoryId)
    {
        var category = await _db.Categories.Include(c => c.Event).FirstOrDefaultAsync(c => c.CategoryID == categoryId);
        if (category == null) return NotFound();
        ViewBag.Category = category;
        var results = await _db.Results.Include(r => r.Enrolment).ThenInclude(e => e!.Participant)
            .Where(r => r.Enrolment!.CategoryID == categoryId && r.Status == "Finished")
            .OrderBy(r => r.Position).ThenBy(r => r.FinishTime).ToListAsync();
        return View(results);
    }
}
