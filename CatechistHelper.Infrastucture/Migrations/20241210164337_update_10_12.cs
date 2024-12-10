using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_10_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "leave_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    leave_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    approver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    approval_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_leave_request_account_approver_id",
                        column: x => x.approver_id,
                        principalTable: "account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_leave_request_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_leave_request_slot_SlotId",
                        column: x => x.SlotId,
                        principalTable: "slot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_approver_id",
                table: "leave_request",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_catechist_id",
                table: "leave_request",
                column: "catechist_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_SlotId",
                table: "leave_request",
                column: "SlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leave_request");
        }
    }
}
