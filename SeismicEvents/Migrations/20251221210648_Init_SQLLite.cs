using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeismicEventsFireEvents.Migrations
{
    /// <inheritdoc />
    public partial class Init_SQLLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeismicCompressed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlynnRegion = table.Column<string>(type: "TEXT", nullable: false),
                    ChunkId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CompressionType = table.Column<string>(type: "TEXT", nullable: false),
                    MinimumDepth = table.Column<double>(type: "REAL", nullable: true),
                    MaximumDepth = table.Column<double>(type: "REAL", nullable: true),
                    MinimumMagnitude = table.Column<double>(type: "REAL", nullable: true),
                    MaximumMagnitude = table.Column<double>(type: "REAL", nullable: true),
                    MinimumLongitude = table.Column<double>(type: "REAL", nullable: true),
                    MaximumLongitude = table.Column<double>(type: "REAL", nullable: true),
                    MinimumLatitude = table.Column<double>(type: "REAL", nullable: true),
                    MaximumLatitude = table.Column<double>(type: "REAL", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstEventDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastEventDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompressedEventProperties = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeismicCompressed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeismicProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceId = table.Column<string>(type: "TEXT", nullable: false),
                    SourceCatalog = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FlynnRegion = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Depth = table.Column<double>(type: "REAL", nullable: true),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Magnitude = table.Column<double>(type: "REAL", nullable: true),
                    MagnitudeType = table.Column<string>(type: "TEXT", nullable: false),
                    Unid = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeismicProperties", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeismicCompressed");

            migrationBuilder.DropTable(
                name: "SeismicProperties");
        }
    }
}
