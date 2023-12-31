using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Models
{
    public partial class TaskDbContext : DbContext
    {
        public TaskDbContext()
        {
        }

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.Details)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.Due_Date)
                    .HasColumnType("datetime")
                    .HasColumnName("Due_Date");
                entity.Property(e => e.Frequency_Number).HasColumnName("Frequency_Number");
                entity.Property(e => e.Frequency_Type)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Frequency_Type");
                entity.Property(e => e.Last_Completed)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_Completed");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
