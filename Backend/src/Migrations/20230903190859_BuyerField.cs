using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class BuyerField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "TblItems",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TblItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BuyerId", "CreationDate", "UpdateDate" },
                values: new object[] { null, new DateTime(2023, 9, 3, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(2331), new DateTime(2023, 9, 4, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(2333) });

            migrationBuilder.UpdateData(
                table: "TblItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bid", "BuyerId", "CreationDate", "UpdateDate" },
                values: new object[] { 0m, null, new DateTime(2023, 9, 3, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(2342), new DateTime(2023, 9, 4, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(2343) });

            migrationBuilder.UpdateData(
                table: "TblUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "Password", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 3, 19, 8, 58, 649, DateTimeKind.Utc).AddTicks(2384), "$2a$11$Ax5kzfVqrkXOJ40EPGnVf.QVTNTUwULTfaYbBUBa3dcoEYdZIvIEa", new DateTime(2023, 9, 3, 19, 8, 58, 649, DateTimeKind.Utc).AddTicks(2388) });

            migrationBuilder.UpdateData(
                table: "TblUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "Password", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 3, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(1440), "$2a$11$d4T7idMx2GJmqBgKHn/5peLhX9blJRntbr1u9OFa4Egj76iFeSTzG", new DateTime(2023, 9, 3, 19, 8, 58, 916, DateTimeKind.Utc).AddTicks(1444) });

            migrationBuilder.CreateIndex(
                name: "IX_TblItems_BuyerId",
                table: "TblItems",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblItems_TblUsers_BuyerId",
                table: "TblItems",
                column: "BuyerId",
                principalTable: "TblUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblItems_TblUsers_BuyerId",
                table: "TblItems");

            migrationBuilder.DropIndex(
                name: "IX_TblItems_BuyerId",
                table: "TblItems");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "TblItems");

            migrationBuilder.UpdateData(
                table: "TblItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 2, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(6237), new DateTime(2023, 9, 3, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "TblItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bid", "CreationDate", "UpdateDate" },
                values: new object[] { 10m, new DateTime(2023, 9, 2, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(6251), new DateTime(2023, 9, 3, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(6251) });

            migrationBuilder.UpdateData(
                table: "TblUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "Password", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 2, 13, 37, 48, 173, DateTimeKind.Utc).AddTicks(280), "$2a$11$afe.dL42f5nL7N7Y99Nh/ehOhsLHhEpe/FPrg3H0R9CtwVhfBiR2.", new DateTime(2023, 9, 2, 13, 37, 48, 173, DateTimeKind.Utc).AddTicks(283) });

            migrationBuilder.UpdateData(
                table: "TblUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "Password", "UpdateDate" },
                values: new object[] { new DateTime(2023, 9, 2, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(5396), "$2a$11$lSe4wGaGZmgCkecr1O/EXeZm9LGG1jvcwLxeXyrQXRML5QmDQ986W", new DateTime(2023, 9, 2, 13, 37, 48, 453, DateTimeKind.Utc).AddTicks(5399) });
        }
    }
}
