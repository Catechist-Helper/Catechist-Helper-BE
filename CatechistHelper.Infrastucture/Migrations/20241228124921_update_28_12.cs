using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatechistHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_28_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "christian_name",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    holy_day = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_christian_name", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "level",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    hierarchy_level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_level", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "major",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    hierarchy_level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_major", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pastoral_year",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pastoral_year", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "post_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "registration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    is_teaching_before = table.Column<bool>(type: "bit", nullable: false),
                    year_of_teaching = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    image_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_configuration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_configuration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_cost = table.Column<double>(type: "float", nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    event_category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_event_category_event_category_id",
                        column: x => x.event_category_id,
                        principalTable: "event_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificate",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    image_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate", x => x.id);
                    table.ForeignKey(
                        name: "FK_certificate_level_level_id",
                        column: x => x.level_id,
                        principalTable: "level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "grade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    major_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade", x => x.id);
                    table.ForeignKey(
                        name: "FK_grade_major_major_id",
                        column: x => x.major_id,
                        principalTable: "major",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teaching_qualification",
                columns: table => new
                {
                    major_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teaching_qualification", x => new { x.level_id, x.major_id });
                    table.ForeignKey(
                        name: "FK_teaching_qualification_level_level_id",
                        column: x => x.level_id,
                        principalTable: "level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teaching_qualification_major_major_id",
                        column: x => x.major_id,
                        principalTable: "major",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificate_of_candidate",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    registration_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_of_candidate", x => x.id);
                    table.ForeignKey(
                        name: "FK_certificate_of_candidate_registration_registration_id",
                        column: x => x.registration_id,
                        principalTable: "registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interview",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    meeting_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_passed = table.Column<bool>(type: "bit", nullable: false),
                    registration_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    interview_type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interview", x => x.id);
                    table.ForeignKey(
                        name: "FK_interview_registration_registration_id",
                        column: x => x.registration_id,
                        principalTable: "registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registration_process",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    registration_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registration_process", x => x.id);
                    table.ForeignKey(
                        name: "FK_registration_process_registration_registration_id",
                        column: x => x.registration_id,
                        principalTable: "registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    hashed_password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    avatar = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "budget_transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    from_budget = table.Column<double>(type: "float", nullable: false),
                    to_budget = table.Column<double>(type: "float", nullable: false),
                    transaction_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_budget_transaction_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participant_in_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_attended = table.Column<bool>(type: "bit", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participant_in_event", x => x.id);
                    table.ForeignKey(
                        name: "FK_participant_in_event_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "process",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fee = table.Column<double>(type: "float", nullable: false),
                    actual_fee = table.Column<double>(type: "float", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process", x => x.id);
                    table.ForeignKey(
                        name: "FK_process_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "training_list",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    previous_level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    next_level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_list", x => x.id);
                    table.ForeignKey(
                        name: "FK_training_list_certificate_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_training_list_level_next_level_id",
                        column: x => x.next_level_id,
                        principalTable: "level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_training_list_level_previous_level_id",
                        column: x => x.previous_level_id,
                        principalTable: "level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    number_of_catechist = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    pastoral_year_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    grade_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_grade_grade_id",
                        column: x => x.grade_id,
                        principalTable: "grade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_class_pastoral_year_pastoral_year_id",
                        column: x => x.pastoral_year_id,
                        principalTable: "pastoral_year",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "catechist",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    birth_place = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    father_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    father_phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    mother_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mother_phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    image_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    phone = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    qualification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_teaching = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    christian_name_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catechist", x => x.id);
                    table.ForeignKey(
                        name: "FK_catechist_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_catechist_christian_name_christian_name_id",
                        column: x => x.christian_name_id,
                        principalTable: "christian_name",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_catechist_level_level_id",
                        column: x => x.level_id,
                        principalTable: "level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => new { x.account_id, x.event_id });
                    table.ForeignKey(
                        name: "FK_member_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_member_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_member_role_event_role_event_id",
                        column: x => x.role_event_id,
                        principalTable: "role_event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    module = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    post_category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_post_category_post_category_id",
                        column: x => x.post_category_id,
                        principalTable: "post_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recruiter_in_interview",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    interview_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    online_room_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    evaluation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recruiter_in_interview", x => new { x.account_id, x.interview_id });
                    table.ForeignKey(
                        name: "FK_recruiter_in_interview_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recruiter_in_interview_interview_interview_id",
                        column: x => x.interview_id,
                        principalTable: "interview",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "member_of_process",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    process_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member_of_process", x => new { x.account_id, x.process_id });
                    table.ForeignKey(
                        name: "FK_member_of_process_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_member_of_process_process_process_id",
                        column: x => x.process_id,
                        principalTable: "process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receipt_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    process_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_receipt_image_process_process_id",
                        column: x => x.process_id,
                        principalTable: "process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "slot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    class_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slot", x => x.id);
                    table.ForeignKey(
                        name: "FK_slot_class_class_id",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_slot_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "catechist_in_class",
                columns: table => new
                {
                    class_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catechist_in_class", x => new { x.catechist_id, x.class_id });
                    table.ForeignKey(
                        name: "FK_catechist_in_class_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_catechist_in_class_class_class_id",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "catechist_in_grade",
                columns: table => new
                {
                    grade_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catechist_in_grade", x => new { x.catechist_id, x.grade_id });
                    table.ForeignKey(
                        name: "FK_catechist_in_grade_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_catechist_in_grade_grade_grade_id",
                        column: x => x.grade_id,
                        principalTable: "grade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "catechist_in_training",
                columns: table => new
                {
                    training_list_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catechist_in_training", x => new { x.catechist_id, x.training_list_id });
                    table.ForeignKey(
                        name: "FK_catechist_in_training_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_catechist_in_training_training_list_training_list_id",
                        column: x => x.training_list_id,
                        principalTable: "training_list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "certificate_of_catechist",
                columns: table => new
                {
                    certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    granted_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_of_catechist", x => new { x.catechist_id, x.certificate_id });
                    table.ForeignKey(
                        name: "FK_certificate_of_catechist_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_certificate_of_catechist_certificate_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                });

            migrationBuilder.CreateTable(
                name: "absence_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    slot_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    replacement_catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    approver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    approval_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_absence_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_absence_request_account_approver_id",
                        column: x => x.approver_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absence_request_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absence_request_catechist_replacement_catechist_id",
                        column: x => x.replacement_catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_absence_request_slot_slot_id",
                        column: x => x.slot_id,
                        principalTable: "slot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "catechist_in_slot",
                columns: table => new
                {
                    slot_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    catechist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catechist_in_slot", x => new { x.catechist_id, x.slot_id });
                    table.ForeignKey(
                        name: "FK_catechist_in_slot_catechist_catechist_id",
                        column: x => x.catechist_id,
                        principalTable: "catechist",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_catechist_in_slot_slot_slot_id",
                        column: x => x.slot_id,
                        principalTable: "slot",
                        principalColumn: "id");
                });

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
                name: "IX_absence_request_approver_id",
                table: "absence_request",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "IX_absence_request_catechist_id",
                table: "absence_request",
                column: "catechist_id");

            migrationBuilder.CreateIndex(
                name: "IX_absence_request_replacement_catechist_id",
                table: "absence_request",
                column: "replacement_catechist_id");

            migrationBuilder.CreateIndex(
                name: "IX_absence_request_slot_id",
                table: "absence_request",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_email",
                table: "account",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_role_id",
                table: "account",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_budget_transaction_event_id",
                table: "budget_transaction",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_account_id",
                table: "catechist",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_catechist_christian_name_id",
                table: "catechist",
                column: "christian_name_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_level_id",
                table: "catechist",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_in_class_class_id",
                table: "catechist_in_class",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_in_grade_grade_id",
                table: "catechist_in_grade",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_in_slot_slot_id",
                table: "catechist_in_slot",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_catechist_in_training_training_list_id",
                table: "catechist_in_training",
                column: "training_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificate_level_id",
                table: "certificate",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificate_of_candidate_registration_id",
                table: "certificate_of_candidate",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificate_of_catechist_certificate_id",
                table: "certificate_of_catechist",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_grade_id",
                table: "class",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_pastoral_year_id",
                table: "class",
                column: "pastoral_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_event_category_id",
                table: "event",
                column: "event_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_grade_major_id",
                table: "grade",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_interview_registration_id",
                table: "interview",
                column: "registration_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_approver_id",
                table: "leave_request",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_catechist_id",
                table: "leave_request",
                column: "catechist_id");

            migrationBuilder.CreateIndex(
                name: "IX_member_event_id",
                table: "member",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_member_role_event_id",
                table: "member",
                column: "role_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_member_of_process_process_id",
                table: "member_of_process",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_participant_in_event_event_id",
                table: "participant_in_event",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_account_id",
                table: "post",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_post_category_id",
                table: "post",
                column: "post_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_process_event_id",
                table: "process",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_image_process_id",
                table: "receipt_image",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_recruiter_in_interview_interview_id",
                table: "recruiter_in_interview",
                column: "interview_id");

            migrationBuilder.CreateIndex(
                name: "IX_registration_process_registration_id",
                table: "registration_process",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_image_request_id",
                table: "request_image",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_slot_class_id",
                table: "slot",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_slot_room_id",
                table: "slot",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_qualification_major_id",
                table: "teaching_qualification",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_list_certificate_id",
                table: "training_list",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_list_next_level_id",
                table: "training_list",
                column: "next_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_list_previous_level_id",
                table: "training_list",
                column: "previous_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_image_budget_transaction_id",
                table: "transaction_image",
                column: "budget_transaction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catechist_in_class");

            migrationBuilder.DropTable(
                name: "catechist_in_grade");

            migrationBuilder.DropTable(
                name: "catechist_in_slot");

            migrationBuilder.DropTable(
                name: "catechist_in_training");

            migrationBuilder.DropTable(
                name: "certificate_of_candidate");

            migrationBuilder.DropTable(
                name: "certificate_of_catechist");

            migrationBuilder.DropTable(
                name: "leave_request");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "member_of_process");

            migrationBuilder.DropTable(
                name: "participant_in_event");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "receipt_image");

            migrationBuilder.DropTable(
                name: "recruiter_in_interview");

            migrationBuilder.DropTable(
                name: "registration_process");

            migrationBuilder.DropTable(
                name: "request_image");

            migrationBuilder.DropTable(
                name: "system_configuration");

            migrationBuilder.DropTable(
                name: "teaching_qualification");

            migrationBuilder.DropTable(
                name: "transaction_image");

            migrationBuilder.DropTable(
                name: "training_list");

            migrationBuilder.DropTable(
                name: "role_event");

            migrationBuilder.DropTable(
                name: "post_category");

            migrationBuilder.DropTable(
                name: "process");

            migrationBuilder.DropTable(
                name: "interview");

            migrationBuilder.DropTable(
                name: "absence_request");

            migrationBuilder.DropTable(
                name: "budget_transaction");

            migrationBuilder.DropTable(
                name: "certificate");

            migrationBuilder.DropTable(
                name: "registration");

            migrationBuilder.DropTable(
                name: "catechist");

            migrationBuilder.DropTable(
                name: "slot");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "christian_name");

            migrationBuilder.DropTable(
                name: "level");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "event_category");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "grade");

            migrationBuilder.DropTable(
                name: "pastoral_year");

            migrationBuilder.DropTable(
                name: "major");
        }
    }
}
