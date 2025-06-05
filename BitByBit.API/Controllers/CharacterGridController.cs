using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitByBit.API.Data;
using BitByBit.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitByBit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterGridController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CharacterGridController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<CharacterGrid>> SaveCharacterGrid(CharacterGridSaveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new character grid
            var characterGrid = new CharacterGrid
            {
                PlayerId = request.PlayerId,
                Name = request.Name,
                GridSize = request.GridSize,
                CreatedAt = DateTime.UtcNow,
                Cells = new List<GridCell>()
            };

            // Add all cells, including empty ones
            for (int y = 0; y < request.GridSize; y++)
            {
                for (int x = 0; x < request.GridSize; x++)
                {
                    var cell = new GridCell
                    {
                        X = x,
                        Y = y,
                        PixelType = request.Cells.FirstOrDefault(c => c.X == x && c.Y == y)?.PixelType
                    };
                    characterGrid.Cells.Add(cell);
                }
            }

            _context.CharacterGrids.Add(characterGrid);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterGrid), new { id = characterGrid.Id }, characterGrid);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterGrid>> GetCharacterGrid(int id)
        {
            var characterGrid = await _context.CharacterGrids
                .Include(cg => cg.Cells)
                .FirstOrDefaultAsync(cg => cg.Id == id);

            if (characterGrid == null)
            {
                return NotFound();
            }

            return characterGrid;
        }

        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<IEnumerable<CharacterGrid>>> GetPlayerCharacterGrids(int playerId)
        {
            return await _context.CharacterGrids
                .Include(cg => cg.Cells)
                .Where(cg => cg.PlayerId == playerId)
                .OrderByDescending(cg => cg.CreatedAt)
                .ToListAsync();
        }
    }

    public class CharacterGridSaveRequest
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int GridSize { get; set; }
        public List<GridCellSaveRequest> Cells { get; set; }
    }

    public class GridCellSaveRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PixelType? PixelType { get; set; }
    }
} 