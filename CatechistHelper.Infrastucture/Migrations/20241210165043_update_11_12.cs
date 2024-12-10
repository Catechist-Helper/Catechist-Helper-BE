using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_11_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_request_slot_SlotId",
                table: "leave_request");

            migrationBuilder.DropIndex(
                name: "IX_leave_request_SlotId",
                table: "leave_request");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "leave_request");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "leave_request",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_SlotId",
                table: "leave_request",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_leave_request_slot_SlotId",
                table: "leave_request",
                column: "SlotId",
                principalTable: "slot",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
