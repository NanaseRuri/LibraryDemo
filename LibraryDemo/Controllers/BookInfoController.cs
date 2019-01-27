using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibraryDemo.Data;
using LibraryDemo.Infrastructure;
using LibraryDemo.Models;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.FileProviders;

namespace LibraryDemo.Controllers
{
    public class BookInfoController : Controller
    {
        private LendingInfoDbContext _context;
        private static int amout = 4;

        public BookInfoController(LendingInfoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            IEnumerable<BookDetails> books = null;
            if (HttpContext.Session != null)
            {
                books = HttpContext.Session.Get<IEnumerable<BookDetails>>("bookDetails");
            }
            if (books == null)
            {
                books = _context.BooksDetail;
                HttpContext.Session?.Set<IEnumerable<BookDetails>>("books", books);
            }
            BookListViewModel model = new BookListViewModel()
            {
                PagingInfo = new PagingInfo()
                {
                    ItemsPerPage = amout,
                    TotalItems = books.Count(),
                    CurrentPage = page,
                },
                BookDetails = books.OrderBy(b => b.FetchBookNumber).Skip((page - 1) * amout).Take(amout)
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBookDetails(BookDetails model)
        {
            if (model == null)
            {
                model = new BookDetails();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBookDetails(BookDetails model, IFormFile image)
        {
            BookDetails bookDetails = new BookDetails();
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    bookDetails.ImageMimeType = image.ContentType;
                    bookDetails.ImageData = new byte[image.Length];
                    await image.OpenReadStream().ReadAsync(bookDetails.ImageData, 0, (int)image.Length);
                }

                bookDetails.ISBN = model.ISBN;
                bookDetails.Name = model.Name;
                bookDetails.Author = model.Author;
                bookDetails.Description = model.Description;
                bookDetails.FetchBookNumber = model.FetchBookNumber;
                bookDetails.Press = model.Press;
                bookDetails.PublishDateTime = model.PublishDateTime;
                bookDetails.SoundCassettes = model.SoundCassettes;
                bookDetails.Version = model.Version;

                await _context.BooksDetail.AddAsync(bookDetails);


                _context.SaveChanges();
                TempData["message"] = $"已添加书籍《{model.Name}》";
                return RedirectToAction("EditBookDetails");
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveBook(IEnumerable<string> isbns)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var isbn in isbns)
            {
                BookDetails bookDetails = _context.BooksDetail.First(b => b.ISBN == isbn);
                IQueryable<Book> books = _context.Books.Where(b => b.ISBN == isbn);
                _context.BooksDetail.Remove(bookDetails);
                _context.Books.RemoveRange(books);
                sb.Append("《" + bookDetails.Name + "》");
                await _context.SaveChangesAsync();
            }

            TempData["message"] = $"已移除书籍{sb.ToString()}";
            return RedirectToAction("EditBookDetails");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string isbn, int page = 1)
        {
            IEnumerable<BookDetails> books = null;
            BookListViewModel model;
            if (HttpContext.Session != null)
            {
                books = HttpContext.Session.Get<IEnumerable<BookDetails>>("bookDetails");
            }
            if (books == null)
            {
                books = _context.BooksDetail;
                HttpContext.Session?.Set<IEnumerable<BookDetails>>("books", books);

            }
            if (isbn != null)
            {
                model = new BookListViewModel()
                {
                    BookDetails = new List<BookDetails>() { books.FirstOrDefault(b => b.ISBN == isbn) },
                    PagingInfo = new PagingInfo()
                };
                return View(model);
            }
            model = new BookListViewModel()
            {

                PagingInfo = new PagingInfo()
                {
                    ItemsPerPage = amout,
                    TotalItems = books.Count(),
                    CurrentPage = page,
                },
                BookDetails = books.OrderBy(b => b.FetchBookNumber).Skip((page - 1) * amout).Take(amout)
            };
            return View(model);
        }

        public IActionResult LendBook()
        {
            return View();
        }

        public FileContentResult GetImage(string isbn)
        {
            BookDetails target = _context.BooksDetail.FirstOrDefault(b => b.ISBN == isbn);
            if (target != null)
            {
                return File(target.ImageData, target.ImageMimeType);
            }
            return null;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBookDetails(string isbn)
        {
            BookDetails book = await _context.BooksDetail.FirstOrDefaultAsync(b => b.ISBN == isbn);
            if (book != null)
            {
                return View(book);
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditBookDetails(BookDetails model, IFormFile image)
        {
            BookDetails bookDetails = _context.BooksDetail.FirstOrDefault(b => b.ISBN == model.ISBN);
            if (ModelState.IsValid)
            {
                if (bookDetails != null)
                {
                    if (image != null)
                    {
                        bookDetails.ImageMimeType = image.ContentType;
                        bookDetails.ImageData = new byte[image.Length];
                        await image.OpenReadStream().ReadAsync(bookDetails.ImageData, 0, (int)image.Length);
                    }

                    BookDetails newBookDetails = model;

                    bookDetails.Name = newBookDetails.Name;
                    bookDetails.Author = newBookDetails.Author;
                    bookDetails.Description = newBookDetails.Description;
                    bookDetails.FetchBookNumber = newBookDetails.FetchBookNumber;
                    bookDetails.Press = newBookDetails.Press;
                    bookDetails.PublishDateTime = newBookDetails.PublishDateTime;
                    bookDetails.SoundCassettes = newBookDetails.SoundCassettes;
                    bookDetails.Version = newBookDetails.Version;

                    _context.SaveChanges();
                    TempData["message"] = $"《{newBookDetails.Name}》修改成功";
                    return RedirectToAction("EditBookDetails");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Search(string keyWord, string value)
        {
            BookDetails bookDetails = new BookDetails();
            switch (keyWord)
            {
                case "Name":
                    bookDetails =await _context.BooksDetail.FirstOrDefaultAsync(b => b.Name == value);
                    break;
                case "ISBN":
                    bookDetails =await _context.BooksDetail.FirstOrDefaultAsync(b => b.ISBN == value);
                    break;
                case "FetchBookNumber":
                    bookDetails =await _context.BooksDetail.FirstOrDefaultAsync(b => b.FetchBookNumber == value);
                    break;
            }

            if (bookDetails!=null)
            {
                return RedirectToAction("EditBookDetails", new {isbn = bookDetails.ISBN});
            }

            TempData["message"] = "找不到该书籍";
            return RedirectToAction("Edit");
        }
    }
}