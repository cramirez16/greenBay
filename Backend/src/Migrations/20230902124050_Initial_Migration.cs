﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Money = table.Column<decimal>(type: "numeric", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Bid = table.Column<decimal>(type: "numeric", nullable: false),
                    IsSellable = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblItems_TblUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "TblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblBids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BidAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BidderId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblBids_TblItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TblItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblBids_TblUsers_BidderId",
                        column: x => x.BidderId,
                        principalTable: "TblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUsersBids",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BidId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsersBids", x => new { x.UserId, x.BidId });
                    table.ForeignKey(
                        name: "FK_TblUsersBids_TblBids_BidId",
                        column: x => x.BidId,
                        principalTable: "TblBids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblUsersBids_TblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TblUsers",
                columns: new[] { "Id", "CreationDate", "Email", "Money", "Name", "Password", "Role", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 2, 12, 40, 50, 290, DateTimeKind.Utc).AddTicks(3227), "admin@fox.hu", 100.00m, "admin", "$2a$11$5r1/aQyXtVmiKrdO1ofawuQC5S6iUu1k1BJgeHbSwu8Qw.HJd0lem", "Admin", new DateTime(2023, 9, 2, 12, 40, 50, 290, DateTimeKind.Utc).AddTicks(3231) },
                    { 2, new DateTime(2023, 9, 2, 12, 40, 50, 633, DateTimeKind.Utc).AddTicks(1903), "testuser@abc.de", 100.00m, "testuser", "$2a$11$neucoBWYzxGfMk64OyWYTu4tY3I0agIFQ84.MqDggz59cY1Ifh5IO", "User", new DateTime(2023, 9, 2, 12, 40, 50, 633, DateTimeKind.Utc).AddTicks(1908) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblBids_BidderId",
                table: "TblBids",
                column: "BidderId");

            migrationBuilder.CreateIndex(
                name: "IX_TblBids_ItemId",
                table: "TblBids",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TblItems_SellerId",
                table: "TblItems",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUsersBids_BidId",
                table: "TblUsersBids",
                column: "BidId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblUsersBids");

            migrationBuilder.DropTable(
                name: "TblBids");

            migrationBuilder.DropTable(
                name: "TblItems");

            migrationBuilder.DropTable(
                name: "TblUsers");
        }
    }
}
