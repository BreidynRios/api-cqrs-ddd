using Domain.Common.Interfaces;

namespace Application.DTOs.Request
{
    public class Request : IPagination
    {
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public bool? IsAscending { get; set; }
        public string? SortingField { get; set; }

        public bool IsValid() => Page.HasValue && PageSize.HasValue;
    }
}
