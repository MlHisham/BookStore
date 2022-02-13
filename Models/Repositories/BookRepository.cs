using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id=1,Title = "C# Programming",Description="No description",ImageUrl="image.jpeg",Author = new Author{Id=2 }
                },
                new Book
                {
                    Id=2,Title = "python Programming",Description="Nothing",ImageUrl="image.png",Author = new Author()

                },
                new Book
                {
                    Id=3,Title = "Java Programming",Description="data description",ImageUrl="istockphoto-873507500-612x612.jpg",Author = new Author()
                }
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(book => book.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book =Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            return books.SingleOrDefault(book => book.Id == id);
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.
                Where(b => b.Title.Contains(term) ||
                        b.Description.Contains(term) ||
                        b.Author.FullName.Contains(term)).ToList();
        }

        public void Update(Book newBook, int id)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
