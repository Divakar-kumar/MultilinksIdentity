using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class UpdateHubConnectionSchema2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "HubConnections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessLevel",
                table: "HubConnections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "HubConnections",
                nullable: true);
        }
    }
}
