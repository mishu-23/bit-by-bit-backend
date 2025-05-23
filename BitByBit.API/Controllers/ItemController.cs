using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitByBit.API.Data;
using BitByBit.API.Models;

namespace BitByBit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/Item
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            try
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            // Update properties
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Type = item.Type;
            existingItem.Rarity = item.Rarity;
            existingItem.Value = item.Value;
            existingItem.HealthBonus = item.HealthBonus;
            existingItem.EnergyBonus = item.EnergyBonus;
            existingItem.DamageBonus = item.DamageBonus;
            existingItem.DefenseBonus = item.DefenseBonus;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
} 