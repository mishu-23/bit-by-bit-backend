using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitByBit.API.Data;
using BitByBit.API.Models;

namespace BitByBit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory/5/items
        [HttpGet("{playerId}/items")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems(int playerId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Items)
                    .ThenInclude(ii => ii.Item)
                .FirstOrDefaultAsync(i => i.PlayerId == playerId);

            if (inventory == null)
            {
                return NotFound("Player inventory not found");
            }

            var items = inventory.Items.Select(ii => new InventoryItemDto
            {
                Id = ii.Id,
                ItemId = ii.ItemId,
                Quantity = ii.Quantity,
                SlotPosition = ii.SlotPosition,
                Item = new ItemDto
                {
                    Id = ii.Item.Id,
                    Name = ii.Item.Name,
                    Description = ii.Item.Description,
                    Type = ii.Item.Type,
                    Rarity = ii.Item.Rarity,
                    Value = ii.Item.Value,
                    HealthBonus = ii.Item.HealthBonus,
                    EnergyBonus = ii.Item.EnergyBonus,
                    DamageBonus = ii.Item.DamageBonus,
                    DefenseBonus = ii.Item.DefenseBonus
                }
            }).ToList();

            return items;
        }

        // POST: api/Inventory/5/items
        [HttpPost("{playerId}/items")]
        public async Task<ActionResult<InventoryItem>> AddItemToInventory(int playerId, [FromBody] AddItemRequest request)
        {
            // Find player's inventory
            var inventory = await _context.Inventories
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.PlayerId == playerId);

            if (inventory == null)
            {
                return NotFound("Player inventory not found");
            }

            // Check if item exists
            var item = await _context.Items.FindAsync(request.ItemId);
            if (item == null)
            {
                return NotFound("Item not found");
            }

            // Check inventory capacity
            if (inventory.Items.Count >= inventory.Capacity)
            {
                return BadRequest("Inventory is full");
            }

            // Check if item already exists in inventory
            var existingItem = inventory.Items.FirstOrDefault(ii => ii.ItemId == request.ItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                // Add new item to inventory
                var inventoryItem = new InventoryItem
                {
                    InventoryId = inventory.Id,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity
                };
                inventory.Items.Add(inventoryItem);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(inventory.Items.FirstOrDefault(ii => ii.ItemId == request.ItemId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Inventory/5/items/10
        [HttpDelete("{playerId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromInventory(int playerId, int itemId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.PlayerId == playerId);

            if (inventory == null)
            {
                return NotFound("Player inventory not found");
            }

            var inventoryItem = inventory.Items.FirstOrDefault(ii => ii.ItemId == itemId);
            if (inventoryItem == null)
            {
                return NotFound("Item not found in inventory");
            }

            inventory.Items.Remove(inventoryItem);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Inventory/5/items/10
        [HttpPut("{playerId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItemQuantity(int playerId, int itemId, [FromBody] UpdateQuantityRequest request)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.PlayerId == playerId);

            if (inventory == null)
            {
                return NotFound("Player inventory not found");
            }

            var inventoryItem = inventory.Items.FirstOrDefault(ii => ii.ItemId == itemId);
            if (inventoryItem == null)
            {
                return NotFound("Item not found in inventory");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            inventoryItem.Quantity = request.Quantity;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(inventoryItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class InventoryItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int SlotPosition { get; set; }
        public ItemDto Item { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public ItemRarity Rarity { get; set; }
        public int Value { get; set; }
        public int? HealthBonus { get; set; }
        public int? EnergyBonus { get; set; }
        public int? DamageBonus { get; set; }
        public int? DefenseBonus { get; set; }
    }

    public class AddItemRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateQuantityRequest
    {
        public int Quantity { get; set; }
    }
} 