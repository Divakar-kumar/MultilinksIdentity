using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Multilinks.DataService.Migrations
{
    public partial class TreatGatewayAsClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectionCapability",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "IsCloudConnected",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "IsGateway",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "ServiceAreaId",
                table: "Endpoints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DirectionCapability",
                table: "Endpoints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCloudConnected",
                table: "Endpoints",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGateway",
                table: "Endpoints",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceAreaId",
                table: "Endpoints",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
