using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_25_11_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "certificate_id",
                table: "training_list",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_training_list_certificate_id",
                table: "training_list",
                column: "certificate_id");

            migrationBuilder.AddForeignKey(
                name: "FK_training_list_certificate_certificate_id",
                table: "training_list",
                column: "certificate_id",
                principalTable: "certificate",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_training_list_certificate_certificate_id",
                table: "training_list");

            migrationBuilder.DropIndex(
                name: "IX_training_list_certificate_id",
                table: "training_list");

            migrationBuilder.DropColumn(
                name: "certificate_id",
                table: "training_list");
        }
    }
}
