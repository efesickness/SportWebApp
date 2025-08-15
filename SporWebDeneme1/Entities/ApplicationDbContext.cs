using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporWebDeneme1.Entities.Models;
using System.Reflection.Emit;

namespace SporWebDeneme1.Entities
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSession> CourseSessions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<StudentSubscription> StudentSubscriptions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchAssignment> BranchAssignments { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<TrainingAssistant> TrainingAssistants { get; set; }
        public DbSet<TrainingStaff> TrainingStaffs { get; set; }
        public DbSet<TrainingSessionSeries> TrainingSessionSeries { get; set; }
        public DbSet<TrainingSessionSeriesDay> TrainingSessionSeriesDays { get; set; }
        public DbSet<CourseSessionDay> CourseSessionDays { get; set; }
        public DbSet<TrainingAttendance> TrainingAttendances { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<EmailConfig> EmailConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.CourseSession)
                .WithMany(cs => cs.Registrations)
                .HasForeignKey(r => r.CourseSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.ApplicationUser)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.IdentityRole)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }
    }
}

