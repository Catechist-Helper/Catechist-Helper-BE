using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_24_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "slot");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "slot");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "slot");

            migrationBuilder.AddColumn<string>(
                name: "RoomUrl",
                table: "recruiter_in_interview",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "interview",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "interview_type",
                table: "interview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "catechist",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "catechist",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "budget_transaction",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "transaction_at",
                table: "budget_transaction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomUrl",
                table: "recruiter_in_interview");

            migrationBuilder.DropColumn(
                name: "interview_type",
                table: "interview");

            migrationBuilder.DropColumn(
                name: "note",
                table: "budget_transaction");

            migrationBuilder.DropColumn(
                name: "transaction_at",
                table: "budget_transaction");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "slot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "slot",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "slot",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "interview",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "catechist",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "catechist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
