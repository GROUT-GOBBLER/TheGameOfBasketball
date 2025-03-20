using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using basketballAPI.models;

namespace basketballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsTypesController : ControllerBase
    {
        private readonly SoftwareBirbContext _context;

        public StatsTypesController(SoftwareBirbContext context)
        {
            _context = context;
        }

        // GET: api/StatsTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatsType>>> GetStatsTypes()
        {
            return await _context.StatsTypes.ToListAsync();
        }

        // GET: api/StatsTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatsType>> GetStatsType(int id)
        {
            var statsType = await _context.StatsTypes.FindAsync(id);

            if (statsType == null)
            {
                return NotFound();
            }

            return statsType;
        }

        // PUT: api/StatsTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatsType(int id, StatsType statsType)
        {
            if (id != statsType.StatId)
            {
                return BadRequest();
            }

            _context.Entry(statsType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatsTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StatsTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatsType>> PostStatsType(StatsType statsType)
        {
            _context.StatsTypes.Add(statsType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatsType", new { id = statsType.StatId }, statsType);
        }

        // DELETE: api/StatsTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatsType(int id)
        {
            var statsType = await _context.StatsTypes.FindAsync(id);
            if (statsType == null)
            {
                return NotFound();
            }

            _context.StatsTypes.Remove(statsType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatsTypeExists(int id)
        {
            return _context.StatsTypes.Any(e => e.StatId == id);
        }
    }
}
