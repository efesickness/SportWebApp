using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SporWebDeneme1.Email;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using SporWebDeneme1.Identity;
using SporWebDeneme1.Identity.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); // RememberMe varsa geçerli olur
    options.SlidingExpiration = false; // Süre bitince oturum sonlansýn

    options.Events.OnValidatePrincipal = context =>
    {
        return Task.CompletedTask;
    };
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero; // Þifre deðiþikliklerini anýnda kontrol et
});

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteStudent", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanDeleteStudent")));

    options.AddPolicy("CanDeleteStudentFromCourseSession", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanDeleteStudentFromCourseSession")));

    options.AddPolicy("CanAddAndDeletePermission", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddAndDeletePermission")));

    options.AddPolicy("CanAccessAndChangeSettings", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessAndChangeSettings")));

    options.AddPolicy("CanAccessUsers", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAccessUsers")));

    options.AddPolicy("CanAddUser", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddUser")));

    options.AddPolicy("CanDeleteUser", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanDeleteUser")));

    options.AddPolicy("CanAssignRoleToUser", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAssignRoleToUser")));

    options.AddPolicy("CanAccessRole", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAccessRole")));

    options.AddPolicy("CanAddAndDeleteRole", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAddAndDeleteRole")));

    options.AddPolicy("CanAccessAdminPanel", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessAdminPanel")));

    options.AddPolicy("CanAccessInstructorPanel", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAccessInstructorPanel")));

    options.AddPolicy("CanAccessCourse", policy =>
        policy.Requirements.Add(new PermissionRequirement("CanAccessCourse")));

    options.AddPolicy("CanAddEditAndDeleteCourse", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddEditAndDeleteCourse")));

    options.AddPolicy("CanActivateDeactiveCourse", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanActivateDeactiveCourse")));

    options.AddPolicy("CanAccessCourseSession", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessCourseSession")));

    options.AddPolicy("CanAddEditAndDeleteCourseSession", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddEditAndDeleteCourseSession")));

    options.AddPolicy("CanAddStudentToCourseSession", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddStudentToCourseSession")));

    options.AddPolicy("CanAccessBranches", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessBranches")));

    options.AddPolicy("CanAddEditAndDeleteBranches", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddEditAndDeleteBranches")));

    options.AddPolicy("CanAccessBranchAssignments", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessBranchAssignments")));

    options.AddPolicy("CanAddEditAndDeleteBranchAssignments", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddEditAndDeleteBranchAssignments")));

    options.AddPolicy("CanAccessMyStudentsPanel", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessMyStudentsPanel")));

    options.AddPolicy("CanAccessTrainingPanel", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessTrainingPanel")));

    options.AddPolicy("CanAddEditAndDeleteTraining", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAddEditAndDeleteTraining")));

    options.AddPolicy("CanAccessSubscriptionsPanel", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessSubscriptionsPanel")));

    options.AddPolicy("CanRenewSubscription", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanRenewSubscription")));

    options.AddPolicy("CanRemoveSubscription", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanRemoveSubscription")));

    options.AddPolicy("CanActivateDeactivateSubscription", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanActivateDeactivateSubscription")));

    options.AddPolicy("CanAccessPendingRegistration", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessPendingRegistration")));

    options.AddPolicy("CanApproveAndRejectPendingRegistration", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanApproveAndRejectPendingRegistration")));

    options.AddPolicy("CanAccessStudentPanel", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanAccessStudentPanel")));

    options.AddPolicy("CanTakeTrainingAttendance", policy =>
    policy.Requirements.Add(new PermissionRequirement("CanTakeTrainingAttendance")));
});

builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();


builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    //error handling section
}
else
{
    app.UseDeveloperExceptionPage();
}

await DataSeeder.SeedCitiesAndDistrictsAsync(app.Services);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityServices.SeedRolesAsync(services);

    services = scope.ServiceProvider;
    await DataSeeder.SeedPermissionsAndAdminAsync(services);

}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
        );

    endpoints.MapAreaControllerRoute(
        name: "Instructor",
        areaName: "Instructor",
        pattern: "Instructor/{controller=Home}/{action=Index}/{id?}"
        );

    endpoints.MapAreaControllerRoute(
        name: "Course",
        areaName: "Course",
        pattern: "Course/{controller=Course}/{action=Index}/{id?}"
        );

    endpoints.MapAreaControllerRoute(
        name: "Student",
        areaName: "Student",
        pattern: "Student/{controller=Home}/{action=Index}/{id?}"
        );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );

});


app.Run();
