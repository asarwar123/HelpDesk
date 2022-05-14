using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.api.Migrations
{
    public partial class TicketsInitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    subject = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    message = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    requester = table.Column<string>(type: "TEXT", nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    isSpam = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
