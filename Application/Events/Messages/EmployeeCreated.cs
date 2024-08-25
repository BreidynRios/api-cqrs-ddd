using Application.Events.Interface;

namespace Application.Events.Messages
{
    public class EmployeeCreated : IMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
