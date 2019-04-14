using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class UsingGuidAsPrimaryKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_LinkId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_HubConnections_EndpointId",
                table: "HubConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints");

            migrationBuilder.DropIndex(
                name: "IX_Endpoints_EndpointId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Endpoints");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "LinkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints",
                column: "EndpointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Links",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Endpoints",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Links_LinkId",
                table: "Links",
                column: "LinkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HubConnections_EndpointId",
                table: "HubConnections",
                column: "EndpointId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_EndpointId",
                table: "Endpoints",
                column: "EndpointId",
                unique: true);
        }
    }
}
