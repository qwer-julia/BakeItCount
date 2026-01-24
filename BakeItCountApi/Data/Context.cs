using BakeItCountApi.Cn.Users;
using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.Votes;
using BakeItCountApi.Cn.Swaps;
using BakeItCountApi.Cn.Achievements;
using Microsoft.EntityFrameworkCore;
using BakeItCountApi.Data.Seeds;
using BakeItCountApi.Cn.FlavorVotes;

namespace BakeItCountApi.Data
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Flavor> Flavors { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Swap> Swaps { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<FlavorVote> FlavorVotes { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.Email)
                      .IsUnique();
            });

            modelBuilder.Entity<Pair>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(p => p.User1)
                      .WithMany()
                      .HasForeignKey(p => p.UserId1)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.User2)
                      .WithMany()
                      .HasForeignKey(p => p.UserId2)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Confirmed)
                      .HasDefaultValue(false);

                entity.HasOne(s => s.Pair)
                      .WithMany(p => p.Schedules)
                      .HasForeignKey(s => s.PairId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne(p => p.Schedule)
                      .WithMany(s => s.Purchases)
                      .HasForeignKey(p => p.ScheduleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Flavor1)
                      .WithMany()
                      .HasForeignKey(p => p.Flavor1Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Flavor2)
                      .WithMany()
                      .HasForeignKey(p => p.Flavor2Id)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Flavor>(entity =>
            {
                entity.Property(f => f.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasMany(f => f.FlavorVotes)
                        .WithOne(fv => fv.Flavor)
                        .HasForeignKey(fv => fv.FlavorId)
                        .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasOne(v => v.User)
                      .WithMany()
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Flavor)
                      .WithMany()
                      .HasForeignKey(v => v.FlavorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(v => v.WeekRef)
                      .IsRequired();
            });

            modelBuilder.Entity<Swap>(entity =>
            {
                entity.Property(s => s.RequestedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(s => s.Status)
                      .HasConversion<string>()
                      .HasDefaultValue(SwapStatus.PENDING);

                entity.HasOne(s => s.SourceSchedule)
                      .WithMany()
                      .HasForeignKey(s => s.SourceScheduleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.TargetSchedule)
                      .WithMany()
                      .HasForeignKey(s => s.TargetScheduleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(ua => new { ua.UserId, ua.AchievementId });

                entity.Property(ua => ua.EarnedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(ua => ua.User)
                      .WithMany()
                      .HasForeignKey(ua => ua.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ua => ua.Achievement)
                      .WithMany()
                      .HasForeignKey(ua => ua.AchievementId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Flavor>()
                        .Property(f => f.Category)
                        .HasConversion<string>();

            modelBuilder.Entity<FlavorVote>(entity =>
            {
                entity.HasOne(fv => fv.User)
                      .WithMany()
                      .HasForeignKey(fv => fv.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(fv => fv.Flavor)
                      .WithMany()
                      .HasForeignKey(fv => fv.FlavorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(fv => fv.VotedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            AchievementSeed.Seed(modelBuilder);
            FlavorSeed.Seed(modelBuilder);

        }
    }
}
