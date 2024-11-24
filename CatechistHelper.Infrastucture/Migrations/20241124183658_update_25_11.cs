using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_25_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomUrl",
                table: "recruiter_in_interview");

            migrationBuilder.AddColumn<string>(
                name: "RoomUrl",
                table: "recruiter",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomUrl",
                table: "recruiter");

            migrationBuilder.AddColumn<string>(
                name: "RoomUrl",
                table: "recruiter_in_interview",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
