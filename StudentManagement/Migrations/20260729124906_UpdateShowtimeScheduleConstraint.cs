using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShowtimeScheduleConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Showtimes_MovieCode",
                table: "Showtimes");

            migrationBuilder.CreateIndex(
                name: "UQ_Showtimes_MovieCode_ShowDateTime",
                table: "Showtimes",
                columns: new[] { "MovieCode", "ShowDateTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Showtimes_MovieCode_ShowDateTime",
                table: "Showtimes");

            migrationBuilder.CreateIndex(
                name: "UQ_Showtimes_MovieCode",
                table: "Showtimes",
                column: "MovieCode",
                unique: true);
        }
    }
}
