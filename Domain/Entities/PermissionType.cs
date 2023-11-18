using Domain.Common;

namespace Domain.Entities
{
    public class PermissionType : Entity
    {
        public PermissionType()
        {
            Permissions = new List<Permission>();
        }
        public string Name { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
