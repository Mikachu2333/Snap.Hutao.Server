﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Hutao.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeparateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "hutao_logs");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "hutao_logs");

            migrationBuilder.CreateTable(
                name: "hutao_log_items",
                columns: table => new
                {
                    PrimaryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LogId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Time = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hutao_log_items", x => x.PrimaryId);
                    table.ForeignKey(
                        name: "FK_hutao_log_items_hutao_logs_LogId",
                        column: x => x.LogId,
                        principalTable: "hutao_logs",
                        principalColumn: "PrimaryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_hutao_log_items_LogId",
                table: "hutao_log_items",
                column: "LogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hutao_log_items");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "hutao_logs",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "hutao_logs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
