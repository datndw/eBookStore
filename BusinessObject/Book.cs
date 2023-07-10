using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int PublisherId { get; set; }
        public double Price { get; set; }
        public double Advance { get; set; }
        public double Royalty { get; set; }
        public double YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime? PublishedDate { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
