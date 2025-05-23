using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitByBit.API.Data;
using BitByBit.API.Models;

namespace BitByBit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Stats)
                .ToListAsync();
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Stats)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // POST: api/Player
        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            try
            {
                // Set timestamps
                player.CreatedAt = DateTime.UtcNow;
                player.LastLogin = DateTime.UtcNow;

                // Create inventory
                var inventory = new Inventory
                {
                    Capacity = player.Inventory?.Capacity ?? 20,
                    Items = new List<InventoryItem>()
                };

                // Create stats
                var stats = new PlayerStats
                {
                    Level = player.Stats?.Level ?? 1,
                    Experience = player.Stats?.Experience ?? 0,
                    Health = player.Stats?.Health ?? 100,
                    MaxHealth = player.Stats?.MaxHealth ?? 100,
                    Energy = player.Stats?.Energy ?? 100,
                    MaxEnergy = player.Stats?.MaxEnergy ?? 100
                };

                // Set relationships
                player.Inventory = inventory;
                player.Stats = stats;

                // Add everything to context
                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error creating player: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingPlayer = await _context.Players
                .Include(p => p.Inventory)
                .Include(p => p.Stats)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPlayer == null)
            {
                return NotFound();
            }

            // Update basic player properties
            existingPlayer.Username = player.Username;
            existingPlayer.LastLogin = DateTime.UtcNow;

            // Update inventory if provided
            if (player.Inventory != null)
            {
                existingPlayer.Inventory.Capacity = player.Inventory.Capacity;
            }

            // Update stats if provided
            if (player.Stats != null)
            {
                existingPlayer.Stats.Level = player.Stats.Level;
                existingPlayer.Stats.Experience = player.Stats.Experience;
                existingPlayer.Stats.Health = player.Stats.Health;
                existingPlayer.Stats.MaxHealth = player.Stats.MaxHealth;
                existingPlayer.Stats.Energy = player.Stats.Energy;
                existingPlayer.Stats.MaxEnergy = player.Stats.MaxEnergy;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingPlayer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
} 