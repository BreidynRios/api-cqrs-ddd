namespace Application.Commons.Settings
{
    public class UserIdentityModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public virtual string[]? Roles { get; set; }
        public bool IsEmployee { get; set; }
    }
}
