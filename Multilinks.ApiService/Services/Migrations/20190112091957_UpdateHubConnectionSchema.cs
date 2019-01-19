using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class UpdateHubConnectionSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessLevel",
                table: "HubConnections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "HubConnections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HubConnections_EndpointId",
                table: "HubConnections",
                column: "EndpointId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HubConnections_EndpointId",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "HubConnections");
        }
    }
}
