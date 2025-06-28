using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cd_task_queue",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "cs_task_schedules",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "cs_task_statuses",
                schema: "jobs");

            migrationBuilder.CreateTable(
                name: "cs_queue_item_statuses",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    c_name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    c_code = table.Column<string>(type: "text", nullable: false, comment: "Код"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_queue_item_statuses", x => x.id);
                    table.CheckConstraint("chk_cs_task_statuses_c_code_valid", "c_code ~ '^[a-zA-Z0-9_-]+$'");
                },
                comment: "Статусы задачи");

            migrationBuilder.CreateTable(
                name: "cs_queue_schedules",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    c_name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    c_description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    c_cron_expression = table.Column<string>(type: "text", nullable: false, comment: "Cron-выражение"),
                    b_is_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак активности"),
                    n_max_items_per_iteration = table.Column<int>(type: "integer", nullable: false, defaultValue: 50, comment: "Количество задач из очереди, обрабатываемых за один проход"),
                    n_concurrency_limit = table.Column<int>(type: "integer", nullable: false, defaultValue: 20, comment: "Количество параллельных задач, обрабатываемых за один проход"),
                    j_custom_params = table.Column<string>(type: "jsonb", nullable: true, comment: "Дополнительные параметры в формате JSON"),
                    n_max_tries = table.Column<int>(type: "integer", nullable: false, defaultValue: 3, comment: "Количество попыток запуска задачи"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_queue_schedules", x => x.id);
                    table.CheckConstraint("chk_cs_task_schedules_c_name_valid", "c_name ~ '^[a-zA-Z0-9_-]+$'");
                },
                comment: "Конфигурация очереди задач");

            migrationBuilder.CreateTable(
                name: "cd_queue_items",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    j_payload = table.Column<string>(type: "jsonb", nullable: false, comment: "Параметры запуска в формате JSON"),
                    j_error = table.Column<string>(type: "jsonb", nullable: true, comment: "Информация об ошибках в формате JSON"),
                    d_start_by_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата и временя после которой запустить"),
                    d_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата запуска"),
                    d_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата завершения"),
                    n_retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Количество запусков"),
                    b_is_manual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак ручного запуска"),
                    f_schedule = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор конфигурации"),
                    f_status = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор статуса"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cd_queue_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_cd_queue_items_cs_queue_item_statuses_f_status",
                        column: x => x.f_status,
                        principalSchema: "jobs",
                        principalTable: "cs_queue_item_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cd_queue_items_cs_queue_schedules_f_schedule",
                        column: x => x.f_schedule,
                        principalSchema: "jobs",
                        principalTable: "cs_queue_schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Очередь задач");

            migrationBuilder.CreateIndex(
                name: "IX_cd_queue_items_f_schedule",
                schema: "jobs",
                table: "cd_queue_items",
                column: "f_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_cd_queue_items_f_status",
                schema: "jobs",
                table: "cd_queue_items",
                column: "f_status");

            migrationBuilder.CreateIndex(
                name: "IX_cs_queue_item_statuses_c_code",
                schema: "jobs",
                table: "cs_queue_item_statuses",
                column: "c_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cs_queue_schedules_c_name",
                schema: "jobs",
                table: "cs_queue_schedules",
                column: "c_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cd_queue_items",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "cs_queue_item_statuses",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "cs_queue_schedules",
                schema: "jobs");

            migrationBuilder.CreateTable(
                name: "cs_task_schedules",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    n_concurrency_limit = table.Column<int>(type: "integer", nullable: false, defaultValue: 20, comment: "Количество параллельных задач, обрабатываемых за один проход"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    c_cron_expression = table.Column<string>(type: "text", nullable: false, comment: "Cron-выражение"),
                    c_description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    b_is_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак активности"),
                    j_custom_params = table.Column<string>(type: "jsonb", nullable: true, comment: "Дополнительные параметры в формате JSON"),
                    n_max_items_per_iteration = table.Column<int>(type: "integer", nullable: false, defaultValue: 50, comment: "Количество задач из очереди, обрабатываемых за один проход"),
                    n_max_retries = table.Column<int>(type: "integer", nullable: false, defaultValue: 3, comment: "Количество попыток повторного запуска задачи"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    c_name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_task_schedules", x => x.id);
                    table.CheckConstraint("chk_cs_task_schedules_c_name_valid", "c_name ~ '^[a-zA-Z0-9_-]+$'");
                },
                comment: "Конфигурация задач");

            migrationBuilder.CreateTable(
                name: "cs_task_statuses",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    c_code = table.Column<string>(type: "text", nullable: false, comment: "Код"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    c_name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_task_statuses", x => x.id);
                    table.CheckConstraint("chk_cs_task_statuses_c_code_valid", "c_code ~ '^[a-zA-Z0-9_-]+$'");
                },
                comment: "Статусы задач");

            migrationBuilder.CreateTable(
                name: "cd_task_queue",
                schema: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()", comment: "Идентификатор"),
                    f_schedule = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор конфигурации"),
                    f_status = table.Column<Guid>(type: "uuid", nullable: false, comment: "Идентификатор статуса"),
                    s_create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата создания"),
                    s_creator = table.Column<string>(type: "text", nullable: true, comment: "Создатель"),
                    d_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата завершения"),
                    b_is_manual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Признак ручного запуска"),
                    j_error = table.Column<string>(type: "jsonb", nullable: true, comment: "Информация об ошибке в формате JSON"),
                    j_payload = table.Column<string>(type: "jsonb", nullable: false, comment: "Параметры запуска в формате JSON"),
                    s_modif_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата изменения"),
                    s_owner = table.Column<string>(type: "text", nullable: true, comment: "Владелец"),
                    n_retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Количество попыток перезапуска задачи"),
                    d_start_by_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Запуск по времени (дата и время запуска)"),
                    d_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата запуска")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cd_task_queue", x => x.id);
                    table.ForeignKey(
                        name: "FK_cd_task_queue_cs_task_schedules_f_schedule",
                        column: x => x.f_schedule,
                        principalSchema: "jobs",
                        principalTable: "cs_task_schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cd_task_queue_cs_task_statuses_f_status",
                        column: x => x.f_status,
                        principalSchema: "jobs",
                        principalTable: "cs_task_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Очередь задач");

            migrationBuilder.CreateIndex(
                name: "IX_cd_task_queue_f_schedule",
                schema: "jobs",
                table: "cd_task_queue",
                column: "f_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_cd_task_queue_f_status",
                schema: "jobs",
                table: "cd_task_queue",
                column: "f_status");

            migrationBuilder.CreateIndex(
                name: "IX_cs_task_schedules_c_name",
                schema: "jobs",
                table: "cs_task_schedules",
                column: "c_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cs_task_statuses_c_code",
                schema: "jobs",
                table: "cs_task_statuses",
                column: "c_code",
                unique: true);
        }
    }
}
