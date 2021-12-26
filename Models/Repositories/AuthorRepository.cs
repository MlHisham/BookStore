using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        IList<Author> authors;

        public AuthorRepository()
        {
            authors = new List<Author>() { new Author { Id=1 ,FullName="Khaled Mohamed"},
            new Author { Id=2 ,FullName="Mohamed Hisham"},
            new Author { Id=3 ,FullName="Ahmed Elamry"}};
        }
        public void Add(Author entity)
        {
            entity.Id = authors.Max(author => author.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            return authors.SingleOrDefault(author => author.Id == id);
        }

        public IList<Author> List()
        {
            return authors;
        }

        public void Update(Author newAuthor, int id)
        {
            var author = Find(id);
            author.FullName = newAuthor.FullName;
        }
    }
}
