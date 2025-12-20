using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeismicEventsFireEvents.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "SeismicCompressed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeismicCompressed",
                table: "SeismicCompressed",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeismicCompressed",
                table: "SeismicCompressed");

            migrationBuilder.RenameTable(
                name: "SeismicCompressed",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }
    }
}
