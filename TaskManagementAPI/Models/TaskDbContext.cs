using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Models;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnString");

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
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("Due_Date");
            entity.Property(e => e.FrequencyNumber).HasColumnName("Frequency_Number");
            entity.Property(e => e.FrequencyType)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("Frequency_Type");
            entity.Property(e => e.LastCompleted)
                .HasColumnType("datetime")
                .HasColumnName("Last_Completed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
