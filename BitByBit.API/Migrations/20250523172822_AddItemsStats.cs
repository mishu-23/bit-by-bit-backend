using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitByBit.API.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStackable",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SpritePath",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "MaxStackSize",
                table: "Items",
                newName: "Value");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "DamageBonus",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefenseBonus",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyBonus",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HealthBonus",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DamageBonus",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DefenseBonus",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EnergyBonus",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HealthBonus",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Items",
                newName: "MaxStackSize");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "IsStackable",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpritePath",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
