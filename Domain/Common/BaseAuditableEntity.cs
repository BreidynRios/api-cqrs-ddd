using Domain.Common.Interfaces;

namespace Domain.Common
{
    public class BaseAuditableEntity : Entity, IAuditableEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDateOnUtc { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateOnUtc { get; set; }
    }
}
