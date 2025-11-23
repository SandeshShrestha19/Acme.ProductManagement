using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class Created_Inventory_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "AppProducts");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "AppOrderItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentStock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInventories_AppProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderItems_ProductId",
                table: "AppOrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInventories_ProductId",
                table: "AppInventories",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems");

            migrationBuilder.DropTable(
                name: "AppInventories");

            migrationBuilder.DropIndex(
                name: "IX_AppOrderItems_ProductId",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "AppOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "AppProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
