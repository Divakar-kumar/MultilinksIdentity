using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilinks.ApiService.Services.Migrations
{
    public partial class UpdateEndpointEntityWithNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Endpoints");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Endpoints",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Endpoints",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ClientEndpointClientId",
                table: "Endpoints",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerEndpointOwnerId",
                table: "Endpoints",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    EndpointClientId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<string>(maxLength: 128, nullable: false),
                    ClientType = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.EndpointClientId);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    EndpointOwnerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentityId = table.Column<Guid>(nullable: false),
                    OwnerName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.EndpointOwnerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_ClientEndpointClientId",
                table: "Endpoints",
                column: "ClientEndpointClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_OwnerEndpointOwnerId",
                table: "Endpoints",
                column: "OwnerEndpointOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Endpoints_Clients_ClientEndpointClientId",
                table: "Endpoints",
                column: "ClientEndpointClientId",
                principalTable: "Clients",
                principalColumn: "EndpointClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Endpoints_Owners_OwnerEndpointOwnerId",
                table: "Endpoints",
                column: "OwnerEndpointOwnerId",
                principalTable: "Owners",
                principalColumn: "EndpointOwnerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endpoints_Clients_ClientEndpointClientId",
                table: "Endpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Endpoints_Owners_OwnerEndpointOwnerId",
                table: "Endpoints");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Endpoints_ClientEndpointClientId",
                table: "Endpoints");

            migrationBuilder.DropIndex(
                name: "IX_Endpoints_OwnerEndpointOwnerId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "ClientEndpointClientId",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "OwnerEndpointOwnerId",
                table: "Endpoints");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Endpoints",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Endpoints",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Endpoints",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientType",
                table: "Endpoints",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Endpoints",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Endpoints",
                nullable: true);
        }
    }
}
