using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Entities.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    }

    public static class DefaultPermissions
    {
        public static readonly List<Permission> All = new()
    {
        new Permission { Name = "CanDeleteStudent", Description = "Öğrenci silebilir" },
        new Permission { Name = "CanDeleteStudentFromCourseSession", Description = "Kurs grubundan öğrenci silebilir" },
        new Permission { Name = "CanAddAndDeletePermission", Description = "Yetki ekleyip silebilir" },
        new Permission { Name = "CanAccessAndChangeSettings", Description = "Ayarlar paneline erişebilir" },
        new Permission { Name = "CanAccessUsers", Description = "Kullanıcı listesine erişebilir" },
        new Permission { Name = "CanAddUser", Description = "Kullanıcı ekleyebilir" },
        new Permission { Name = "CanDeleteUser", Description = "Kullanıcı silebilir" },
        new Permission { Name = "CanAssignRoleToUser", Description = "Kullanıcıya rol atayabilir" },
        new Permission { Name = "CanAccessRole", Description = "Rolleri görebilir" },
        new Permission { Name = "CanAddAndDeleteRole", Description = "Rol ekleyip silebilir" },
        new Permission { Name = "CanAccessAdminPanel", Description = "Admin paneline erişebilir" },
        new Permission { Name = "CanAccessInstructorPanel", Description = "Eğitmen paneline erişebilir" },
        new Permission { Name = "CanAccessCourse", Description = "Kurslara erişebilir" },
        new Permission { Name = "CanAddEditAndDeleteCourse", Description = "Kurs ekleyebilir, düzenleyebilir, silebilir" },
        new Permission { Name = "CanActivateDeactiveCourse", Description = "Kursu aktif/pasif yapabilir" },
        new Permission { Name = "CanAccessCourseSession", Description = "Gruplara erişebilir" },
        new Permission { Name = "CanAddEditAndDeleteCourseSession", Description = "Grup ekleyebilir, düzenleyebilir, silebilir" },
        new Permission { Name = "CanAddStudentToCourseSession", Description = "Gruba öğrenci ekleyebilir" },
        new Permission { Name = "CanAccessBranches", Description = "Branşlara erişebilir" },
        new Permission { Name = "CanAddEditAndDeleteBranches", Description = "Branş ekleyebilir, düzenleyebilir, silebilir" },
        new Permission { Name = "CanAccessBranchAssignments", Description = "Branş atamalarına erişebilir" },
        new Permission { Name = "CanAddEditAndDeleteBranchAssignments", Description = "Branş ataması yapabilir" },
        new Permission { Name = "CanAccessMyStudentsPanel", Description = "Eğitmen öğrenci paneline erişebilir" },
        new Permission { Name = "CanAccessTrainingPanel", Description = "Antrenman paneline erişebilir" },
        new Permission { Name = "CanAddEditAndDeleteTraining", Description = "Antrenman ekleyebilir, düzenleyebilir, silebilir" },
        new Permission { Name = "CanAccessSubscriptionsPanel", Description = "Abonelik paneline erişebilir" },
        new Permission { Name = "CanRenewSubscription", Description = "Abonelik yenileyebilir" },
        new Permission { Name = "CanRemoveSubscription", Description = "Abonelik silebilir" },
        new Permission { Name = "CanActivateDeactivateSubscription", Description = "Aboneliği aktif/pasif yapabilir" },
        new Permission { Name = "CanAccessPendingRegistration", Description = "Bekleyen kayıtları görebilir" },
        new Permission { Name = "CanApproveAndRejectPendingRegistration", Description = "Bekleyen kayıtları onaylayabilir/red edebilir" },
        new Permission { Name = "CanAccessStudentPanel", Description = "Öğrenci paneline erişebilir" },
        new Permission { Name = "CanTakeTrainingAttendance", Description = "Yoklama alabilir" }
    };
    }

}
