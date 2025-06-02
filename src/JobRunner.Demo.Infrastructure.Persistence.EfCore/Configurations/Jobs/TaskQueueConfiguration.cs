using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.Configurations;

public class TaskQueueConfiguration : IEntityTypeConfiguration<TaskQueue>
{
    public void Configure(EntityTypeBuilder<TaskQueue> entity)
    {
        entity.ToTable("cd_task_queue", "jobs", tb => 
        {
            tb.HasComment("Очередь задач");
        });

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        entity.Property(e => e.JPayload)
            .HasComment("Параметры запуска в формате JSON")
            .HasColumnType("jsonb")
            .HasColumnName("j_payload")
            .IsRequired();

        entity.Property(e => e.JError)
            .HasComment("Информация об ошибке в формате JSON")
            .HasColumnType("jsonb")
            .HasColumnName("j_error")
            .IsRequired(false);

        entity.Property(e => e.StartByDate)
            .HasComment("Запуск по времени (дата и время запуска)")
            .HasColumnName("d_start_by_date")
            .IsRequired(false);

        entity.Property(e => e.StartDate)
            .HasComment("Дата запуска")
            .HasColumnName("d_start_date")
            .IsRequired(false);

        entity.Property(e => e.EndDate)
            .HasComment("Дата завершения")
            .HasColumnName("d_end_date")
            .IsRequired(false);

        entity.Property(e => e.RetryCount)
            .HasComment("Количество попыток перезапуска задачи")
            .HasColumnName("n_retry_count")
            .HasDefaultValue(0)
            .IsRequired();

        entity.Property(e => e.IsManual)
            .HasComment("Признак ручного запуска")
            .HasColumnName("b_is_manual")
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(e => e.TaskScheduleId)
            .HasComment("Идентификатор конфигурации")
            .HasColumnName("f_schedule")
            .IsRequired();

        entity.Property(e => e.TaskStatusId)
            .HasComment("Идентификатор статуса")
            .HasColumnName("f_status")
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

        entity.HasOne(e => e.TaskSchedule).WithMany(e => e.Tasks)
            .HasForeignKey(e => e.TaskScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.TaskStatus).WithMany(e => e.Tasks)
            .HasForeignKey(e => e.TaskStatusId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
