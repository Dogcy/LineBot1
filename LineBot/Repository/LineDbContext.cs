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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-O15LURE\\SQLEXPRESS01;Initial Catalog=LineDb;Integrated Security=True");
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

            modelBuilder.Entity<JableRecord>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.JableRecords)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JableRecords_User");
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
