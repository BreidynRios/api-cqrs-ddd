namespace Domain.Common.Interfaces
{
    public interface IPagination
    {
        int? PageSize { get; set; }
        int? Page { get; set; }
        bool IsValid();
    }
}
