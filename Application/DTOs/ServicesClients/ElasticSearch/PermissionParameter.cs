namespace Application.DTOs.ServicesClients.ElasticSearch
{
    public class PermissionParameter
    {
        public string OperationName { get; set; }
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateOnUtc { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateOnUtc { get; set; }
    }
}
