using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class HubConnectionsMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HubConnections",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "Connected",
                table: "HubConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionID",
                table: "HubConnections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "HubConnections",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HubConnections",
                table: "HubConnections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_EndpointId",
                table: "Endpoints",
                column: "EndpointId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HubConnections",
                table: "HubConnections");

            migrationBuilder.DropIndex(
                name: "IX_Endpoints_EndpointId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HubConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionID",
                table: "HubConnections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Connected",
                table: "HubConnections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HubConnections",
                table: "HubConnections",
                column: "ConnectionID");
        }
    }
}
