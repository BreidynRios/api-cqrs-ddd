namespace Domain.Common.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        int CreatedBy { get; set; }
        DateTime CreatedDateOnUtc { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedDateOnUtc { get; set; }
    }
}
