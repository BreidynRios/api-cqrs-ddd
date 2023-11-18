using Domain.Common.Interfaces;

namespace Domain.Common
{
    public class BaseAuditableEntity : Entity, IAuditableEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
