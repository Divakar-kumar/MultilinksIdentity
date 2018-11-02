using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class HubConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HubConnections",
                columns: table => new
                {
                    ConnectionID = table.Column<string>(nullable: false),
                    EndpointId = table.Column<Guid>(nullable: false),
                    Connected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubConnections", x => x.ConnectionID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubConnections");
        }
    }
}
