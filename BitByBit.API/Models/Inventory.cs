using System;
using System.Collections.Generic;

namespace BitByBit.API.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Capacity { get; set; }
        
        // Navigation properties
        public virtual Player Player { get; set; }
        public virtual ICollection<InventoryItem> Items { get; set; }
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int SlotPosition { get; set; }
        
        // Navigation properties
        public virtual Inventory Inventory { get; set; }
        public virtual Item Item { get; set; }
    }
} 