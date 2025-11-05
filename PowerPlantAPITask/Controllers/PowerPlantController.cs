using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerPlantAPITask.Data;
using PowerPlantAPITask.Models;
using System.Globalization;
using System.Text;

namespace PowerPlantAPITask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PowerPlantsController : ControllerBase
    {
        private readonly PowerPlantDbContext _context;

        public PowerPlantsController(PowerPlantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPowerPlants([FromQuery] string? owner, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.PowerPlants.AsQueryable();

            if (!string.IsNullOrEmpty(owner))
            {
                owner = owner.ToLowerInvariant();
                query = query.Where(p => p.Owner.ToLower().Contains(owner));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total = totalCount,
                totalPages = Math.Ceiling((decimal) totalCount / pageSize),
                page,
                pageSize,
                powerPlants = items
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddPowerPlant([FromBody] PowerPlant powerPlant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (powerPlant.Power < 0 || powerPlant.Power > 200)
                return BadRequest(new { error = "Power must be between 0 and 200." });

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                powerPlant.Owner ?? "",
                 @"^[A-Za-zÀ-ž]+\s[A-Za-zÀ-ž]+$"))
                return BadRequest(new { error = "Owner must contain two words with letters only, separated by a whitespace." });

            _context.PowerPlants.Add(powerPlant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPowerPlants), new { id = powerPlant.Id }, powerPlant);
        }

        private string NormalizeString(string input)
        {
            string normalizedStr = new string(input.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return normalizedStr;
        }
    }
}
