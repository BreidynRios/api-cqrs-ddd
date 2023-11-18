using Domain.Common;

namespace Domain.Entities
{
    public class Employee : BaseAuditableEntity
    {
        public Employee()
        {
            Permissions = new List<Permission>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
