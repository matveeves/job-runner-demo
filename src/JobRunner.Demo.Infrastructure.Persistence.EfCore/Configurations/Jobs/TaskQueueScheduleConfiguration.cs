using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using JobRunner.Demo.Domain.Entities;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.Configurations;

public class TaskQueueScheduleConfiguration : IEntityTypeConfiguration<TaskQueueSchedule>
{
    public void Configure(EntityTypeBuilder<TaskQueueSchedule> entity)
    {
        entity.ToTable("cs_queue_schedules", "jobs", tb => 
        {
            tb.HasComment("Конфигурация очереди задач");
            tb.HasCheckConstraint("chk_cs_task_schedules_c_name_valid",
                "c_name ~ '^[a-zA-Z0-9_-]+$'");
        });

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        entity.Property(e => e.Name)
            .HasComment("Название")
            .HasColumnName("c_name")
            .IsRequired();

        entity.HasIndex(e => e.Name)
            .IsUnique();

        entity.Property(e => e.Description)
            .HasComment("Описание")
            .HasColumnName("c_description")
            .IsRequired(false);

        entity.Property(e => e.CronExpression)
            .HasComment("Cron-выражение")
            .HasColumnName("c_cron_expression")
            .IsRequired();

        entity.Property(e => e.IsEnabled)
            .HasComment("Признак активности")
            .HasColumnName("b_is_enabled")
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(e => e.MaxItemsPerIteration)
            .HasComment("Количество задач из очереди, обрабатываемых за один проход")
            .HasColumnName("n_max_items_per_iteration")
            .HasDefaultValue(50)
            .IsRequired();

        entity.Property(e => e.ConcurrencyLimitPerIteration)
            .HasComment("Количество параллельных задач, обрабатываемых за один проход")
            .HasColumnName("n_concurrency_limit")
            .HasDefaultValue(20)
            .IsRequired();

        entity.Property(e => e.JCustomParams)
            .HasComment("Дополнительные параметры в формате JSON")
            .HasColumnType("jsonb")
            .HasColumnName("j_custom_params")
            .IsRequired(false);

        entity.Property(e => e.MaxRetries)
            .HasComment("Количество попыток повторного запуска задачи")
            .HasColumnName("n_max_retries")
            .HasDefaultValue(3)
            .IsRequired();

        entity.Property(e => e.CreateDate)
            .HasComment("Дата создания")
            .HasColumnName("s_create_date");

        entity.Property(e => e.ModifDate)
            .HasComment("Дата изменения")
            .HasColumnName("s_modif_date");

        entity.Property(e => e.Creator)
            .HasComment("Создатель")
            .HasColumnName("s_creator");

        entity.Property(e => e.Owner)
            .HasComment("Владелец")
            .HasColumnName("s_owner");
    }
}
