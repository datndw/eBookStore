using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class Role
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
