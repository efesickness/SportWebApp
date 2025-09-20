using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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


            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),    
                d => DateOnly.FromDateTime(d));        

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.BirthDate)
                .HasConversion(dateOnlyConverter);

            modelBuilder.Entity<SiteSettings>()
                .Property(s => s.DefaultCoursePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Course>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .HasOne<IdentityRole>()
                .WithMany()
                .HasForeignKey(rc => rc.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<District>()
                .HasOne(d => d.City)
                .WithMany(c => c.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.District)
                .WithMany()
                .HasForeignKey(u => u.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.IdentityRole)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p=>p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseSession>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseSessions)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.ApplicationUser)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Course)
                .WithMany()
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.CourseSession)
                .WithMany(cs => cs.Registrations)
                .HasForeignKey(r => r.CourseSessionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrainingSessionSeries>()
                .HasOne(ts => ts.ApplicationUser)
                .WithMany()
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrainingSessionSeries>()
                .HasOne(ts => ts.CourseSession)
                .WithMany()
                .HasForeignKey(ts => ts.CourseSessionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.ApplicationUser)
                .WithMany()
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.CourseSession)
                .WithMany()
                .HasForeignKey(ts => ts.CourseSessionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.StudentSubscription)
                .WithMany()
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

