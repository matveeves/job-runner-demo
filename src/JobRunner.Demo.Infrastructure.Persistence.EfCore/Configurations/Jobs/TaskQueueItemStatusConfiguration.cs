using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.Configurations;

public class TaskQueueItemStatusConfiguration : IEntityTypeConfiguration<TaskQueueItemStatus>
{
    public void Configure(EntityTypeBuilder<TaskQueueItemStatus> entity)
    {
        entity.ToTable("cs_queue_item_statuses", "jobs", tb => 
        {
            tb.HasComment("Статусы задачи");
            tb.HasCheckConstraint("chk_cs_task_statuses_c_code_valid",
                "c_code ~ '^[a-zA-Z0-9_-]+$'");
        });

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        entity.Property(e => e.Name)
            .HasComment("Название")
            .HasColumnName("c_name")
            .IsRequired();

        entity.Property(e => e.Code)
            .HasComment("Код")
            .HasColumnName("c_code")
            .HasConversion<string>()
            .IsRequired();

        entity.HasIndex(e => e.Code)
            .IsUnique();

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
