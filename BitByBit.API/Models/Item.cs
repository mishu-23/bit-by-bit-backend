using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BitByBit.API.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public ItemType Type { get; set; }

        [Required]
        public ItemRarity Rarity { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Value { get; set; }

        // Optional stats that the item might provide
        public int? HealthBonus { get; set; }
        public int? EnergyBonus { get; set; }
        public int? DamageBonus { get; set; }
        public int? DefenseBonus { get; set; }

        // Navigation property for inventory items
        [JsonIgnore]
        public ICollection<InventoryItem>? InventoryItems { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Quest,
        Material,
        Misc
    }

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
} 