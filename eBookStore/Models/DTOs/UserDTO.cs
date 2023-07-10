namespace eBookStore.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string? Source { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public string RoleDesc { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public DateTime? HireDate { get; set; }
    }
}
