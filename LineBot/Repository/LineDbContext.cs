using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LineBot.Repository.Models;

#nullable disable

namespace LineBot.Repository
{
    public partial class LineDbContext : DbContext
    {
        public LineDbContext()
        {
        }

        public LineDbContext(DbContextOptions<LineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConsumingRecord> ConsumingRecords { get; set; }
        public virtual DbSet<JableRecord> JableRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<ConsumingRecord>(entity =>
            {
                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.ConsumingRecords)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsumingRecords_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.LineId).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
