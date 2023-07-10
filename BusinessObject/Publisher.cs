using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
