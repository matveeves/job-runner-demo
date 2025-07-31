using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixTaskQueueItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "n_retry_count",
                schema: "jobs",
                table: "cd_queue_items",
                newName: "n_try_count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "n_try_count",
                schema: "jobs",
                table: "cd_queue_items",
                newName: "n_retry_count");
        }
    }
}
