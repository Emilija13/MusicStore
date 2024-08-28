using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStore.Repository
{
    /// <inheritdoc />
    public partial class trackinplaylistupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_UserPlaylists_UserPlaylistId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_UserPlaylistId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "UserPlaylistId",
                table: "Tracks");

            migrationBuilder.CreateTable(
                name: "TrackInPlaylist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackInPlaylist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackInPlaylist_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackInPlaylist_UserPlaylists_UserPlaylistId",
                        column: x => x.UserPlaylistId,
                        principalTable: "UserPlaylists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackInPlaylist_TrackId",
                table: "TrackInPlaylist",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackInPlaylist_UserPlaylistId",
                table: "TrackInPlaylist",
                column: "UserPlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackInPlaylist");

            migrationBuilder.AddColumn<Guid>(
                name: "UserPlaylistId",
                table: "Tracks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_UserPlaylistId",
                table: "Tracks",
                column: "UserPlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_UserPlaylists_UserPlaylistId",
                table: "Tracks",
                column: "UserPlaylistId",
                principalTable: "UserPlaylists",
                principalColumn: "Id");
        }
    }
}
