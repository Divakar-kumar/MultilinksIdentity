using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class UpdateNotificationEntityRelatedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientEndpointId",
                table: "Notifications",
                newName: "RecipientEndpointEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientEndpointEndpointId",
                table: "Notifications",
                column: "RecipientEndpointEndpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Endpoints_RecipientEndpointEndpointId",
                table: "Notifications",
                column: "RecipientEndpointEndpointId",
                principalTable: "Endpoints",
                principalColumn: "EndpointId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Endpoints_RecipientEndpointEndpointId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecipientEndpointEndpointId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "RecipientEndpointEndpointId",
                table: "Notifications",
                newName: "RecipientEndpointId");
        }
    }
}
