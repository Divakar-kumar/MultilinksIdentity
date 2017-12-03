using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Multilinks.ApiService.Data.Migrations
{
    public partial class ChangeEndpointPrimaryKeyDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Endpoints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Endpoints");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Endpoints",
                table: "Endpoints",
                column: "EndpointId");
        }
    }
}
