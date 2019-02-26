using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class LinkTableFieldsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondEndpointId",
                table: "Links",
                newName: "SourceEndpointId");

            migrationBuilder.RenameColumn(
                name: "FirstEndpointId",
                table: "Links",
                newName: "AssociatedEndpointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceEndpointId",
                table: "Links",
                newName: "SecondEndpointId");

            migrationBuilder.RenameColumn(
                name: "AssociatedEndpointId",
                table: "Links",
                newName: "FirstEndpointId");
        }
    }
}
