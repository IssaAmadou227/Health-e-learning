using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutureDocteur.API.Migrations
{
    /// <inheritdoc />
    public partial class MedicShopMigration_v_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "ProductsPhotoPath",
                table: "Stores",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsPhotoPath",
                table: "Stores");
        }
    }
}
