using Domain.Common;

namespace Domain.Entities
{
    public class Permission : BaseAuditableEntity
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int PermissionTypeId { get; set; }
        public virtual PermissionType PermissionType { get; set; }
    }
}
