using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AikaHalli.Data
{
    public class ApplicationDbContext : IdentityDbContext
	{
        //public ApplicationDbContext()
        //{
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TimeEntry> TimeEntries { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("connectionstring-localdb-whatever");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskDescription).HasMaxLength(255);

                entity.Property(e => e.TaskName).HasMaxLength(255);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(e => e.EntryId)
                    .HasName("PK__TimeEntr__F57BD2F7477F2EA5");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EntryDate).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(255);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TimeEntries)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_Tasks_TimeEntries");
            });

            modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

            base.OnModelCreating(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
