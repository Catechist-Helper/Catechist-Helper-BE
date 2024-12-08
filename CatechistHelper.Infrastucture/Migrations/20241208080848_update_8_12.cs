using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_8_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recruiter_in_interview_account_account_id",
                table: "recruiter_in_interview");

            migrationBuilder.DropForeignKey(
                name: "FK_recruiter_in_interview_interview_interview_id",
                table: "recruiter_in_interview");

            migrationBuilder.AddColumn<string>(
                name: "online_room_url",
                table: "recruiter_in_interview",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "process",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<long>(
                name: "duration",
                table: "process",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.CreateTable(
                name: "request_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_request_image_absence_request_request_id",
                        column: x => x.request_id,
                        principalTable: "absence_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_request_image_request_id",
                table: "request_image",
                column: "request_id");

            migrationBuilder.AddForeignKey(
                name: "FK_recruiter_in_interview_account_account_id",
                table: "recruiter_in_interview",
                column: "account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recruiter_in_interview_interview_interview_id",
                table: "recruiter_in_interview",
                column: "interview_id",
                principalTable: "interview",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recruiter_in_interview_account_account_id",
                table: "recruiter_in_interview");

            migrationBuilder.DropForeignKey(
                name: "FK_recruiter_in_interview_interview_interview_id",
                table: "recruiter_in_interview");

            migrationBuilder.DropTable(
                name: "request_image");

            migrationBuilder.DropColumn(
                name: "online_room_url",
                table: "recruiter_in_interview");

            migrationBuilder.AlterColumn<double>(
                name: "note",
                table: "process",
                type: "float",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "duration",
                table: "process",
                type: "time",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "duration_temp",
                table: "process",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_recruiter_in_interview_account_account_id",
                table: "recruiter_in_interview",
                column: "account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_recruiter_in_interview_interview_interview_id",
                table: "recruiter_in_interview",
                column: "interview_id",
                principalTable: "interview",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
