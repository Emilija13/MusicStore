using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStore.Repository
{
    /// <inheritdoc />
    public partial class _addquantity_to_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AlbumsInCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AlbumsInCarts");
        }
    }
}
