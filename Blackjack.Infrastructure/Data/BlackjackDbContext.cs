using Blackjack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Data
{
    public class BlackjackDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
        public DbSet<UserStatistics> UserStatistics => Set<UserStatistics>();
        public BlackjackDbContext(DbContextOptions<BlackjackDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder.Entity<UserCredential>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UserCredential>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<UserCredential>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserStatistics>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UserStatistics>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<UserStatistics>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
