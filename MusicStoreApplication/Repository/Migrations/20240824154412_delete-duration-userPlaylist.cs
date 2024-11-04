using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStore.Repository
{
    /// <inheritdoc />
    public partial class deletedurationuserPlaylist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "UserPlaylists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "UserPlaylists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
