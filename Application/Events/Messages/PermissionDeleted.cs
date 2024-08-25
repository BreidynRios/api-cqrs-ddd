using Application.Events.Interface;

namespace Application.Events.Messages
{
    public class PermissionDeleted : IMessage
    {
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
