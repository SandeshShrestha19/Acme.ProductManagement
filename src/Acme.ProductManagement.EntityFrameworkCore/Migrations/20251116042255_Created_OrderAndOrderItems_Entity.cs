using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.ProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class Created_OrderAndOrderItems_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_AppOrderItems_ProductId",
                table: "AppOrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "AppOrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems",
                column: "OrderId",
                principalTable: "AppOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "AppOrderItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderItems_ProductId",
                table: "AppOrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppOrders_OrderId",
                table: "AppOrderItems",
                column: "OrderId",
                principalTable: "AppOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderItems_AppProducts_ProductId",
                table: "AppOrderItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
