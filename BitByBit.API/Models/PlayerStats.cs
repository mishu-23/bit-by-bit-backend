using System;

namespace BitByBit.API.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Energy { get; set; }
        public int MaxEnergy { get; set; }
        
        // Navigation property
        public virtual Player? Player { get; set; }
    }
} 