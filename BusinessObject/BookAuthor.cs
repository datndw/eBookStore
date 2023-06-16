using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class BookAuthor
    {
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public int AuthorOrder { get; set; }
        public float RoyalityPercentage { get; set; }
        public virtual Author Author { get; set; }
        public virtual Book Book { get; set; }
    }
}
