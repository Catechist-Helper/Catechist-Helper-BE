using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_20_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_absent_request_account_ApproverId",
                table: "absent_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absent_request_catechist_CatechistId",
                table: "absent_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absent_request_catechist_ReplacementCatechistId",
                table: "absent_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absent_request_slot_SlotId",
                table: "absent_request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_absent_request",
                table: "absent_request");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "absent_request");

            migrationBuilder.RenameTable(
                name: "absent_request",
                newName: "absence_request");

            migrationBuilder.RenameColumn(
                name: "room",
                table: "room",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "room",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "role",
                newName: "role_name");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "registration",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "cuurent_budget",
                table: "event",
                newName: "current_budget");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "certificate",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "catechist",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "account",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "absence_request",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "absence_request",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "absence_request",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "SlotId",
                table: "absence_request",
                newName: "slot_id");

            migrationBuilder.RenameColumn(
                name: "ReplacementCatechistId",
                table: "absence_request",
                newName: "replacement_catechist_id");

            migrationBuilder.RenameColumn(
                name: "CatechistId",
                table: "absence_request",
                newName: "catechist_id");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "absence_request",
                newName: "approver_id");

            migrationBuilder.RenameColumn(
                name: "ApprovalDate",
                table: "absence_request",
                newName: "approval_date");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "absence_request",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_absent_request_SlotId",
                table: "absence_request",
                newName: "IX_absence_request_slot_id");

            migrationBuilder.RenameIndex(
                name: "IX_absent_request_ReplacementCatechistId",
                table: "absence_request",
                newName: "IX_absence_request_replacement_catechist_id");

            migrationBuilder.RenameIndex(
                name: "IX_absent_request_CatechistId",
                table: "absence_request",
                newName: "IX_absence_request_catechist_id");

            migrationBuilder.RenameIndex(
                name: "IX_absent_request_ApproverId",
                table: "absence_request",
                newName: "IX_absence_request_approver_id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "training_list",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "training_list",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "system_configuration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "system_configuration",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "room",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "status",
                table: "registration_process",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "registration",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "registration",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "process",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "module",
                table: "post",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "participant_in_event",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddColumn<Guid>(
                name: "event_category_id",
                table: "event",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "certificate_of_candidate",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "certificate",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "catechist_in_slot",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "qualification",
                table: "catechist",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "catechist",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mother_phone",
                table: "catechist",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "catechist",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "father_phone",
                table: "catechist",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "account",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "avatar",
                table: "account",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "status",
                table: "absence_request",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "reason",
                table: "absence_request",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "absence_request",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "absence_request",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "absence_request",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_absence_request",
                table: "absence_request",
                column: "id");

            migrationBuilder.CreateTable(
                name: "event_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_category", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_event_category_id",
                table: "event",
                column: "event_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_absence_request_account_approver_id",
                table: "absence_request",
                column: "approver_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absence_request_catechist_catechist_id",
                table: "absence_request",
                column: "catechist_id",
                principalTable: "catechist",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absence_request_catechist_replacement_catechist_id",
                table: "absence_request",
                column: "replacement_catechist_id",
                principalTable: "catechist",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absence_request_slot_slot_id",
                table: "absence_request",
                column: "slot_id",
                principalTable: "slot",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_event_event_category_event_category_id",
                table: "event",
                column: "event_category_id",
                principalTable: "event_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_absence_request_account_approver_id",
                table: "absence_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absence_request_catechist_catechist_id",
                table: "absence_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absence_request_catechist_replacement_catechist_id",
                table: "absence_request");

            migrationBuilder.DropForeignKey(
                name: "FK_absence_request_slot_slot_id",
                table: "absence_request");

            migrationBuilder.DropForeignKey(
                name: "FK_event_event_category_event_category_id",
                table: "event");

            migrationBuilder.DropTable(
                name: "event_category");

            migrationBuilder.DropIndex(
                name: "IX_event_event_category_id",
                table: "event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_absence_request",
                table: "absence_request");

            migrationBuilder.DropColumn(
                name: "event_category_id",
                table: "event");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "absence_request");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "absence_request");

            migrationBuilder.RenameTable(
                name: "absence_request",
                newName: "absent_request");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "room",
                newName: "room");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "room",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "role_name",
                table: "role",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "registration",
                newName: "fullname");

            migrationBuilder.RenameColumn(
                name: "current_budget",
                table: "event",
                newName: "cuurent_budget");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "certificate",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "catechist",
                newName: "fullname");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "account",
                newName: "fullname");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "absent_request",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "absent_request",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "absent_request",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "slot_id",
                table: "absent_request",
                newName: "SlotId");

            migrationBuilder.RenameColumn(
                name: "replacement_catechist_id",
                table: "absent_request",
                newName: "ReplacementCatechistId");

            migrationBuilder.RenameColumn(
                name: "catechist_id",
                table: "absent_request",
                newName: "CatechistId");

            migrationBuilder.RenameColumn(
                name: "approver_id",
                table: "absent_request",
                newName: "ApproverId");

            migrationBuilder.RenameColumn(
                name: "approval_date",
                table: "absent_request",
                newName: "ApprovalDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "absent_request",
                newName: "UpdateAt");

            migrationBuilder.RenameIndex(
                name: "IX_absence_request_slot_id",
                table: "absent_request",
                newName: "IX_absent_request_SlotId");

            migrationBuilder.RenameIndex(
                name: "IX_absence_request_replacement_catechist_id",
                table: "absent_request",
                newName: "IX_absent_request_ReplacementCatechistId");

            migrationBuilder.RenameIndex(
                name: "IX_absence_request_catechist_id",
                table: "absent_request",
                newName: "IX_absent_request_CatechistId");

            migrationBuilder.RenameIndex(
                name: "IX_absence_request_approver_id",
                table: "absent_request",
                newName: "IX_absent_request_ApproverId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "training_list",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "training_list",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "system_configuration",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "system_configuration",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "image",
                table: "room",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "registration_process",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "registration",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "registration",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "process",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "module",
                table: "post",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "participant_in_event",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "certificate_of_candidate",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "image",
                table: "certificate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "catechist_in_slot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "qualification",
                table: "catechist",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "catechist",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mother_phone",
                table: "catechist",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "catechist",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "father_phone",
                table: "catechist",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "account",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "avatar",
                table: "account",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "absent_request",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "absent_request",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "absent_request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "absent_request",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_absent_request",
                table: "absent_request",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_absent_request_account_ApproverId",
                table: "absent_request",
                column: "ApproverId",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absent_request_catechist_CatechistId",
                table: "absent_request",
                column: "CatechistId",
                principalTable: "catechist",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absent_request_catechist_ReplacementCatechistId",
                table: "absent_request",
                column: "ReplacementCatechistId",
                principalTable: "catechist",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_absent_request_slot_SlotId",
                table: "absent_request",
                column: "SlotId",
                principalTable: "slot",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
