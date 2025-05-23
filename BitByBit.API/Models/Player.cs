using System;
using System.Collections.Generic;

namespace BitByBit.API.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        
        // Navigation properties
        public virtual Inventory Inventory { get; set; }
        public virtual PlayerStats Stats { get; set; }
    }
} 