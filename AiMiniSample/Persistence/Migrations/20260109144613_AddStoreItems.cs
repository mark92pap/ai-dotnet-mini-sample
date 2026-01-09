using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiMiniSample.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Sku = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_Sku",
                table: "StoreItems",
                column: "Sku",
                unique: true,
                filter: "Sku IS NOT NULL");

            // Seed initial store items
            migrationBuilder.InsertData(
                table: "StoreItems",
                columns: new[] { "Name", "Description", "Sku", "Category", "Price", "Currency", "IsActive", "StockQuantity", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { "Premium Dog Food", "High-quality dry dog food for adult dogs", "DF-001", "Food", 49.99m, "USD", true, 100, DateTime.UtcNow, DateTime.UtcNow },
                    { "Cat Toy Mouse", "Interactive plush mouse toy for cats", "TOY-001", "Toys", 9.99m, "USD", true, 250, DateTime.UtcNow, DateTime.UtcNow },
                    { "Dog Leash - Blue", "Durable nylon dog leash, 6 feet", "ACC-001", "Accessories", 14.99m, "USD", true, 75, DateTime.UtcNow, DateTime.UtcNow },
                    { "Cat Litter Box", "Self-cleaning automatic litter box", "ACC-002", "Accessories", 89.99m, "USD", true, 30, DateTime.UtcNow, DateTime.UtcNow },
                    { "Bird Seed Mix", "Premium blend for small birds", "FOOD-002", "Food", 12.99m, "USD", true, 150, DateTime.UtcNow, DateTime.UtcNow },
                    { "Aquarium Filter", "High-efficiency filter for 20-50 gallon tanks", "ACC-003", "Accessories", 34.99m, "USD", true, 45, DateTime.UtcNow, DateTime.UtcNow },
                    { "Dog Collar - Red", "Adjustable collar with quick-release buckle", "ACC-004", "Accessories", 12.99m, "USD", true, 120, DateTime.UtcNow, DateTime.UtcNow },
                    { "Cat Scratching Post", "Tall sisal rope scratching post", "TOY-002", "Toys", 39.99m, "USD", true, 60, DateTime.UtcNow, DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreItems");
        }
    }
}
