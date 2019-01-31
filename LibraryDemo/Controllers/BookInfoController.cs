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
                books = _context.BooksDetail.AsNoTracking();
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
        public IActionResult BookDetails(string isbn, int page = 1)
        {
            IEnumerable<BookDetails> books = null;
            BookListViewModel model;
            if (HttpContext.Session != null)
            {
                books = HttpContext.Session.Get<IEnumerable<BookDetails>>("bookDetails");
            }
            if (books == null)
            {
                books = _context.BooksDetail.AsNoTracking();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBooksAndBookDetails(IEnumerable<string> isbns)
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
        public async Task<IActionResult> EditBookDetails(string isbn)
        {
            BookDetails book = await _context.BooksDetail.FirstOrDefaultAsync(b => b.ISBN == isbn);
            if (book != null)
            {
                return View(book);
            }
            else
            {
                return RedirectToAction("BookDetails");
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

                    await _context.SaveChangesAsync();
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
                    bookDetails = await _context.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.Name == value);
                    break;
                case "ISBN":
                    bookDetails = await _context.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.ISBN == value);
                    break;
                case "FetchBookNumber":
                    bookDetails = await _context.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.FetchBookNumber == value);
                    break;
            }

            if (bookDetails != null)
            {
                return RedirectToAction("EditBookDetails", new { isbn = bookDetails.ISBN });
            }

            TempData["message"] = "找不到该书籍";
            return RedirectToAction("BookDetails");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Books(string isbn)
        {
            BookEditModel model = new BookEditModel()
            {
                Books = _context.Books.AsNoTracking().Where(b => b.ISBN == isbn),
                BookDetails = _context.BooksDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn)
            };
            if (model.BookDetails == null)
            {
                TempData["message"] = "未找到目标书籍";
                return RedirectToAction("BookDetails");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBook(string isbn)
        {
            BookDetails bookDetails = _context.BooksDetail.FirstOrDefault(b => b.ISBN == isbn);
            if (bookDetails == null)
            {
                return RedirectToAction("BookDetails", new { isbn = isbn });
            }
            Book book = new Book()
            {
                ISBN = bookDetails.ISBN,
                Name = bookDetails.Name,
                FetchBookNumber = bookDetails.FetchBookNumber
            };
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBook([Bind("ISBN,Name,FetchBookNumber,BarCode,BookshelfId,State")]Book book)
        {
            if (ModelState.IsValid)
            {
                BookDetails bookDetails = _context.BooksDetail.FirstOrDefault(b => b.ISBN == book.ISBN);
                Book existBook = _context.Books.AsNoTracking().FirstOrDefault(b => b.BarCode == book.BarCode);
                if (existBook != null)
                {
                    TempData["message"] = $"已有二维码为{book.BarCode}的书籍《{existBook.Name}》";
                    return RedirectToAction("AddBook", new { isbn = book.ISBN });
                }
                if (bookDetails.Name == book.Name)
                {
                    Bookshelf bookshelf = _context.Bookshelves.Include(b => b.Books).FirstOrDefault(b => b.BookshelfId == book.BookshelfId);
                    if (bookshelf != null)
                    {
                        book.Sort = bookshelf.Sort;
                        book.Location = bookshelf.Location;
                        bookshelf.Books.Add(book);
                        bookshelf.MaxFetchNumber = bookshelf.Books.Max(b => b.FetchBookNumber);
                        bookshelf.MinFetchNumber = bookshelf.Books.Min(b => b.FetchBookNumber);
                    }
                    await _context.Books.AddAsync(book);
                    await _context.SaveChangesAsync();
                    TempData["message"] = $"《{book.Name}》 {book.BarCode} 添加成功";
                    return RedirectToAction("Books", new { isbn = book.ISBN });
                }
            }
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBooks(IEnumerable<string> barcodes, string isbn)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var barcode in barcodes)
            {
                Book book = _context.Books.First(b => b.BarCode == barcode);
                _context.Books.Remove(book);
                sb.AppendLine($"{book.BarCode} 移除成功");
            }
            await _context.SaveChangesAsync();
            TempData["message"] = sb.ToString();
            return RedirectToAction("Books", new { isbn = isbn });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditBook(string barcode)
        {
            Book book = _context.Books.First(b => b.BarCode == barcode);
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(string oldBarCode, [Bind("BarCode,BookshelfId,BorrowTime,Name,KeeperId,AppointedLatestTime")]Book book)
        {
            if (ModelState.IsValid)
            {
                Book oldBook = _context.Books.FirstOrDefault(b => b.BarCode == oldBarCode);
                if (oldBook == null)
                {
                    ViewBag["message"] = $"不存在二维码为{oldBarCode}的书籍";
                    return RedirectToAction("BookDetails");
                }

                if (oldBook.Name == book.Name)
                {
                    book.ISBN = oldBook.ISBN;
                    book.FetchBookNumber = oldBook.FetchBookNumber;
                    Bookshelf bookshelf = _context.Bookshelves.Include(b => b.Books).FirstOrDefault(b => b.BookshelfId == book.BookshelfId);
                    if (bookshelf != null)
                    {
                        book.Sort = bookshelf.Sort;
                        book.Location = bookshelf.Location;
                        bookshelf.Books.Remove(oldBook);
                        bookshelf.Books.Add(book);
                    }

                    _context.Books.Remove(oldBook);
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "修改成功";
                    return RedirectToAction("Books", new { isbn = oldBook.ISBN });
                }
            }
            return View(book);
        }


        public FileContentResult GetImage(string isbn)
        {
            BookDetails target = _context.BooksDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn);
            if (target != null)
            {
                return File(target.ImageData, target.ImageMimeType);
            }
            return null;
        }
    }
}