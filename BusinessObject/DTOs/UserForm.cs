using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class UserForm
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string? Source { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public int PublisherId { get; set; }
        public DateTime? HireDate { get; set; }
    }
}
