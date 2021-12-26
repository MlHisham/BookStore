using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookAuthorViewModel
    {
        public int BookID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }

        public List<Author> Authors { get; set; }
    }
}
