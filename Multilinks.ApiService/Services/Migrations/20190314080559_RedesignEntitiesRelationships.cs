using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class RedesignEntitiesRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "EndpointId",
                table: "HubConnections");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Links",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "HubConnections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Connected",
                table: "HubConnections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "HubConnectionId",
                table: "Endpoints",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_HubConnectionId",
                table: "Endpoints",
                column: "HubConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Endpoints_HubConnections_HubConnectionId",
                table: "Endpoints",
                column: "HubConnectionId",
                principalTable: "HubConnections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endpoints_HubConnections_HubConnectionId",
                table: "Endpoints");

            migrationBuilder.DropIndex(
                name: "IX_Endpoints_HubConnectionId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Connected",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "HubConnectionId",
                table: "Endpoints");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Links",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "HubConnections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "EndpointId",
                table: "HubConnections",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
