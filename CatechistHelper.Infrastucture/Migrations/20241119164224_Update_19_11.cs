using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_19_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "training_list",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "training_list",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "absent_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatechistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReplacementCatechistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_absent_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_absent_request_account_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absent_request_catechist_CatechistId",
                        column: x => x.CatechistId,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absent_request_catechist_ReplacementCatechistId",
                        column: x => x.ReplacementCatechistId,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absent_request_slot_SlotId",
                        column: x => x.SlotId,
                        principalTable: "slot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_absent_request_ApproverId",
                table: "absent_request",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_absent_request_CatechistId",
                table: "absent_request",
                column: "CatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_absent_request_ReplacementCatechistId",
                table: "absent_request",
                column: "ReplacementCatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_absent_request_SlotId",
                table: "absent_request",
                column: "SlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "absent_request");

            migrationBuilder.DropColumn(
                name: "description",
                table: "training_list");

            migrationBuilder.DropColumn(
                name: "name",
                table: "training_list");
        }
    }
}
