using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string? Source { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public int PublisherId { get; set; }
        public DateTime? HireDate { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Role Role { get; set; }
    }
}
