using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeismicEventsFireEvents.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlynnRegion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChunkId = table.Column<int>(type: "int", nullable: false),
                    EventCount = table.Column<int>(type: "int", nullable: false),
                    CompressionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinDepth = table.Column<double>(type: "float", nullable: true),
                    MaxDepth = table.Column<double>(type: "float", nullable: true),
                    MinMagnitude = table.Column<double>(type: "float", nullable: true),
                    MaxMagnitude = table.Column<double>(type: "float", nullable: true),
                    MinLongitude = table.Column<double>(type: "float", nullable: true),
                    MaxLongitude = table.Column<double>(type: "float", nullable: true),
                    MinLatitude = table.Column<double>(type: "float", nullable: true),
                    MaxLatitude = table.Column<double>(type: "float", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstEventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompressedEventProperties = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
