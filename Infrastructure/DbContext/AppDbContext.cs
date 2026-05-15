using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBContext
{
    public class AppDbContext :IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<RepetedGroup> RepetedGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<GroupItem>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.GroupItem)
                .WithMany(g => g.TodoItems)
                .HasForeignKey(t => t.GroupItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RepetedGroup>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RepetedGroup>()
                .HasOne(r => r.GroupItem)
                .WithMany(g => g.RepetedGroups)
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RepetedGroup>()
                .HasMany(r => r.TodoItems)
                .WithOne(t => t.RepitedGroup)
                .HasForeignKey(t => t.RepitedGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.RepitedGroup) 
                .WithMany(r => r.TodoItems)
                .HasForeignKey(t => t.RepitedGroupId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupItems)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .HasMany(u => u.TodoItems)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TodoItem>()
                .HasIndex(t => t.UserId);

            modelBuilder.Entity<TodoItem>()
                .HasIndex(t => t.GroupItemId);

            modelBuilder.Entity<GroupItem>()
                .HasIndex(g => g.UserId);

            modelBuilder.Entity<RepetedGroup>()
                .HasIndex(r => r.GroupId);

            modelBuilder.Entity<RepetedGroup>()
                .HasIndex(r => r.RepetitionDate);
        }
    }
}
