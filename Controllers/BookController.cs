using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookRepository,IBookStoreRepository<Author> authorRepository,
            IWebHostEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid) { 
                try
                {
                    string filename = string.Empty;
                    if(model.File != null)
                    {
                        var uploads = Path.Combine(hosting.WebRootPath,"Uploads");
                        filename = model.File.FileName;
                        string fullpath = Path.Combine(uploads, filename);
                        model.File.CopyTo(new FileStream(fullpath,FileMode.Create));
                    }
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message= "Please select an author from the list!";
                        
                        return View(GetAllAuthors());
                    }
                    var author = authorRepository.Find(model.AuthorId);
                    var book = new Book {
                        Id = model.BookID,
                        Title = model.Title,
                        Description = model.Description,
                        ImageUrl = filename,
                        Author = author
                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "you have to fill all ther required fields!");
            
            return View(GetAllAuthors());
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorID = book.Author == null ? 0 : book.Author.Id;
            var viewModel = new BookAuthorViewModel
            {
                BookID = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorID,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl,
            };
            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
            try
            {
                string filename = string.Empty;
                if (viewModel.File != null)
                {
                    var uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                    filename = viewModel.File.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    //delete the old file
                    string oldFileName = bookRepository.Find(viewModel.BookID).ImageUrl;
                    string fullOldPath = Path.Combine(uploads, oldFileName);
                    if (fullOldPath != fullpath)
                    {
                        System.IO.File.Delete(fullOldPath);
                        //save the new file 
                        viewModel.File.CopyTo(new FileStream(fullpath, FileMode.Create));
                    }
                }
                var author = authorRepository.Find(viewModel.AuthorId);
                var book = new Book
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = author,
                    ImageUrl = filename,
                };
                bookRepository.Update(book, viewModel.BookID);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "---Please select an author---" });
            return authors;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;
        }
    }
}
