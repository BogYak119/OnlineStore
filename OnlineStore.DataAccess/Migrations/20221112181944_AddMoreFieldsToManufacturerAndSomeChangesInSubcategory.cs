using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreFieldsToManufacturerAndSomeChangesInSubcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Manufacturers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Manufacturers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Manufacturers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Manufacturers");
        }
    }
}
