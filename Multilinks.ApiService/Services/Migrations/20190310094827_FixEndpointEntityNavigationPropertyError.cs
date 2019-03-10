using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class FixEndpointEntityNavigationPropertyError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssociatedEndpointId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "SourceEndpointId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedEndpointEndpointId",
                table: "Links",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceEndpointEndpointId",
                table: "Links",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_AssociatedEndpointEndpointId",
                table: "Links",
                column: "AssociatedEndpointEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_SourceEndpointEndpointId",
                table: "Links",
                column: "SourceEndpointEndpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Endpoints_AssociatedEndpointEndpointId",
                table: "Links",
                column: "AssociatedEndpointEndpointId",
                principalTable: "Endpoints",
                principalColumn: "EndpointId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Endpoints_SourceEndpointEndpointId",
                table: "Links",
                column: "SourceEndpointEndpointId",
                principalTable: "Endpoints",
                principalColumn: "EndpointId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Endpoints_AssociatedEndpointEndpointId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Endpoints_SourceEndpointEndpointId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_AssociatedEndpointEndpointId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_SourceEndpointEndpointId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "AssociatedEndpointEndpointId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "SourceEndpointEndpointId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Links",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedEndpointId",
                table: "Links",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SourceEndpointId",
                table: "Links",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
