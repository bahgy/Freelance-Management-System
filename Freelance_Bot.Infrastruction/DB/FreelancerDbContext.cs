using Freelance_Bot.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance_Bot.Infrastruction.DB
{
    public class FreelancerDbContext : DbContext
    {
        public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options) : base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Insight> insights { get; set; }
        public DbSet<AppEvent> events { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<TaskItem> tasks { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── User ────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.TelegramChatId);
            });

            // ─── Client ──────────────────────────────────────────
            modelBuilder.Entity<Client>(e =>
            {
                e.HasOne(c => c.User)
                 .WithMany(u => u.Clients)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Project ─────────────────────────────────────────
            modelBuilder.Entity<Project>(e =>
            {
                e.HasOne(p => p.User)
                 .WithMany(u => u.Projects)
                 .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.Client)
                 .WithMany(c => c.Projects)
                 .HasForeignKey(p => p.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.Property(p => p.Status)
                 .HasConversion<string>();

                e.HasIndex(p => new { p.UserId, p.Status });
                e.HasIndex(p => p.Deadline);
            });

            // ─── Task ─────────────────────────────────────────────
            modelBuilder.Entity<TaskItem>(e =>
            {
                e.HasOne(t => t.Project)
                 .WithMany(p => p.Tasks)
                 .HasForeignKey(t => t.ProjectId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(t => t.ParentTask)
                 .WithMany(t => t.SubTasks)
                 .HasForeignKey(t => t.ParentTaskId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.Property(t => t.Status)
                 .HasConversion<string>();

                e.Property(t => t.Priority)
                 .HasConversion<string>();

                e.HasIndex(t => new { t.ProjectId, t.Status });
                e.HasIndex(t => t.DueDate);
            });

            // ─── Report ───────────────────────────────────────────
            modelBuilder.Entity<Report>(e =>
            {
                e.HasOne(r => r.Project)
                 .WithMany(p => p.Reports)
                 .HasForeignKey(r => r.ProjectId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(r => r.Type).HasConversion<string>();
                //  e.Property(r => r.).HasConversion<string>();
            });

            // ─── Insight ──────────────────────────────────────────
            modelBuilder.Entity<Insight>(e =>
            {
                e.HasOne(i => i.User)
                 .WithMany(u => u.Insights)
                 .HasForeignKey(i => i.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.Project)
                 .WithMany(p => p.Insights)
                 .HasForeignKey(i => i.ProjectId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.Property(i => i.Category).HasConversion<string>();
                e.Property(i => i.Severity).HasConversion<string>();
                e.HasIndex(i => new { i.UserId, i.IsDismissed });
            });

            // ─── AppEvent ─────────────────────────────────────────
            modelBuilder.Entity<AppEvent>(e =>
            {
                e.HasOne(ev => ev.User)
                 .WithMany(u => u.Events)
                 .HasForeignKey(ev => ev.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(ev => new { ev.IsProcessed, ev.CreatedAt });
                e.HasIndex(ev => ev.EventName);
            });

            // ─── Notification ─────────────────────────────────────
            modelBuilder.Entity<Notification>(e =>
            {
                e.HasOne(n => n.User)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(n => n.Channel).HasConversion<string>();
                e.Property(n => n.Status).HasConversion<string>();
                e.HasIndex(n => new { n.UserId, n.Status });
            });
        }
            public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        if (entry.Properties.Any(p => p.IsModified))
                        {
                            entry.Entity.UpdatedAt = utcNow;
                        }
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = utcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(ct);
        }
    }
    
}