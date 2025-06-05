using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitByBit.API.Models
{
    public class CharacterGrid
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public int GridSize { get; set; }
        public virtual ICollection<GridCell> Cells { get; set; }
        public virtual Player Player { get; set; }
    }

    public class GridCell
    {
        public int Id { get; set; }
        public int CharacterGridId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public PixelType? PixelType { get; set; }
        public virtual CharacterGrid CharacterGrid { get; set; }
    }

    public enum PixelType
    {
        Armor,
        Critical,
        Damage,
        Health,
        Lifesteal,
        Luck
    }
} 