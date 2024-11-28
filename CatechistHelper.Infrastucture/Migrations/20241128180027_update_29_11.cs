using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_29_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recruiter");

            migrationBuilder.DropIndex(
                name: "IX_interview_registration_id",
                table: "interview");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "system_configuration",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "process",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<double>(
                name: "actual_fee",
                table: "process",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "note",
                table: "process",
                type: "float",
                maxLength: 500,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "event",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "transaction_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    budget_transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_image_budget_transaction_budget_transaction_id",
                        column: x => x.budget_transaction_id,
                        principalTable: "budget_transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_interview_registration_id",
                table: "interview",
                column: "registration_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_image_budget_transaction_id",
                table: "transaction_image",
                column: "budget_transaction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_image");

            migrationBuilder.DropIndex(
                name: "IX_interview_registration_id",
                table: "interview");

            migrationBuilder.DropColumn(
                name: "actual_fee",
                table: "process");

            migrationBuilder.DropColumn(
                name: "note",
                table: "process");

            migrationBuilder.DropColumn(
                name: "note",
                table: "event");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "system_configuration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "process",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "recruiter",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registration_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recruiter", x => new { x.account_id, x.registration_id });
                    table.ForeignKey(
                        name: "FK_recruiter_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recruiter_registration_registration_id",
                        column: x => x.registration_id,
                        principalTable: "registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_interview_registration_id",
                table: "interview",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_recruiter_registration_id",
                table: "recruiter",
                column: "registration_id");
        }
    }
}
