namespace Domain.Common.Interfaces
{
    public interface IPagination
    {
        int? PageSize { get; set; }
        int? Page { get; set; }
        bool? IsAscending { get; set; }
        string? SortingField { get; set; }
        bool IsValid();
    }
}
