using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStore.Repository
{
    /// <inheritdoc />
    public partial class updatetrackmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Tracks",
                newName: "Seconds");

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "Seconds",
                table: "Tracks",
                newName: "Duration");
        }
    }
}
