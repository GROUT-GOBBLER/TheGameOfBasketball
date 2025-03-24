using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using basketballAPI.models;

namespace basketballAPI.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamPlayersController : ControllerBase
    {
        private readonly SoftwareBirbContext _context;

        public TeamPlayersController(SoftwareBirbContext context)
        {
            _context = context;
        }

        // GET: api/TeamPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamPlayer>>> GetTeamPlayers()
        {
            return await _context.TeamPlayers.ToListAsync();
        }

        // GET: api/TeamPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamPlayer>> GetTeamPlayer(int id)
        {
            var teamPlayer = await _context.TeamPlayers.FindAsync(id);

            if (teamPlayer == null)
            {
                return NotFound();
            }

            return teamPlayer;
        }

        // PUT: api/TeamPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamPlayer(int id, TeamPlayer teamPlayer)
        {
            if (id != teamPlayer.Id)
            {
                return BadRequest();
            }

            _context.Entry(teamPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamPlayerExists(id))
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

        // POST: api/TeamPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamPlayer>> PostTeamPlayer(TeamPlayer teamPlayer)
        {
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeamPlayer", new { id = teamPlayer.Id }, teamPlayer);
        }

        // DELETE: api/TeamPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamPlayer(int id)
        {
            var teamPlayer = await _context.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            _context.TeamPlayers.Remove(teamPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamPlayerExists(int id)
        {
            return _context.TeamPlayers.Any(e => e.Id == id);
        }
    }
}
