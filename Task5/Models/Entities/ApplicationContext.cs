using Microsoft.EntityFrameworkCore;

namespace Task5.Models.Entities
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Mail> Mails { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mail>(entity =>
            {
                entity.ToTable("mails");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Body)
                    .IsUnicode(false)
                    .HasColumnName("body");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.From).HasColumnName("from");

                entity.Property(e => e.Title)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.To).HasColumnName("to");

                entity.HasOne(d => d.FromNavigation)
                    .WithMany(p => p.MailFromNavigations)
                    .HasForeignKey(d => d.From)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mails_users_from_id_fk");

                entity.HasOne(d => d.ToNavigation)
                    .WithMany(p => p.MailToNavigations)
                    .HasForeignKey(d => d.To)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mails_users_to_id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username, "users_username_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Username)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
