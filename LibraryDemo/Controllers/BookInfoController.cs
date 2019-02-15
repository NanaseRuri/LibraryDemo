using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryDemo.Data;
using LibraryDemo.Infrastructure;
using LibraryDemo.Models;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.FileProviders;

namespace LibraryDemo.Controllers
{
    public class BookInfoController : Controller
    {
        private LendingInfoDbContext _lendingInfoDbContext;
        private static int amout = 4;

        public BookInfoController(LendingInfoDbContext context)
        {
            _lendingInfoDbContext = context;
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
                books = _lendingInfoDbContext.BooksDetail.AsNoTracking();
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

        public IActionResult Detail(string isbn)
        {
            BookDetails bookDetails = _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn);
            if (bookDetails == null)
            {
                TempData["message"] = $"找不到 ISBN 为{isbn}的书籍";
                return View("Index");
            }
            BookEditModel model = new BookEditModel()
            {
                BookDetails = bookDetails,
                Books = _lendingInfoDbContext.Books.Include(b => b.Appointments).AsNoTracking().Where(b => b.ISBN == isbn)
            };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Appointing(string barcode, string isbn)
        {
            Book bookingBook = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == barcode);

            if (bookingBook?.State == BookState.Readonly)
            {
                ModelState.AddModelError("", "该书不供外借");
                return RedirectToAction("Detail", new { isbn = isbn });
            }

            StudentInfo student =
                await _lendingInfoDbContext.Students.Include(s => s.KeepingBooks).ThenInclude(k => k.Book)
                    .FirstOrDefaultAsync(s => s.UserName == User.Identity.Name);
            if (student.AppointingBookBarCode != null)
            {
                TempData["message"] = "您已有预约书籍";
                return RedirectToAction("Detail", new { isbn = isbn });
            }

            if (student.KeepingBooks.Any(b => b.Book.BarCode == barcode))
            {
                TempData["message"] = "您已借阅该书籍";
                return RedirectToAction("Detail", new { isbn = isbn });
            }

            if (bookingBook == null)
            {
                TempData["message"] = $"不存在书籍二维码为{barcode}";
                return RedirectToAction("Detail", new { isbn = isbn });
            }
            else
            {
                return View(bookingBook);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appointing(string barcode, DateTime dateTime)
        {
            Book lendingBook = _lendingInfoDbContext.Books.Include(b => b.Appointments).FirstOrDefault(b => b.BarCode == barcode);
            if (ModelState.IsValid)
            {
                if (dateTime <= DateTime.Now)
                {
                    ModelState.AddModelError("", "你必须选择之后的日期");
                    return View(lendingBook);
                }
                if (lendingBook == null)
                {
                    TempData["message"] = $"不存在二维码为{barcode}的书籍";
                    return RedirectToAction("Detail", new { isbn = lendingBook.ISBN });
                }
                StudentInfo student = await _lendingInfoDbContext.Students.Include(s => s.KeepingBooks).FirstOrDefaultAsync(s => s.UserName == User.Identity.Name);
                lendingBook.State = BookState.Appointed;
                lendingBook.Appointments.Add(new AppointmentOrLending()
                {
                    BookId = lendingBook.BarCode,
                    StudentId = student.UserName,
                    AppointingDateTime = dateTime
                });
                lendingBook.AppointedLatestTime = lendingBook.Appointments.Max(b => b.AppointingDateTime);
                student.AppointingBookBarCode = lendingBook.BarCode;
                await _lendingInfoDbContext.SaveChangesAsync();
                TempData["message"] = "预约成功";
                return RedirectToAction("Detail", new { isbn = lendingBook.ISBN });
            }
            return View(lendingBook);
        }

        [Authorize]
        public async Task<IActionResult> CancelAppointing(string barcode)
        {
            if (ModelState.IsValid)
            {
                StudentInfo student = _lendingInfoDbContext.Students.Include(s => s.KeepingBooks).ThenInclude(k => k.Book).ThenInclude(b => b.Appointments)
                    .FirstOrDefault(s => s.UserName == User.Identity.Name);
                if (student.AppointingBookBarCode == barcode)
                {
                    student.AppointingBookBarCode = null;
                    AppointmentOrLending targetAppointment = student.KeepingBooks.FirstOrDefault(a => a.BookId == barcode);
                    student.KeepingBooks.Remove(targetAppointment);
                    Book targetBook = targetAppointment.Book;

                    targetBook.AppointedLatestTime = targetBook.Appointments.Any(a => a.AppointingDateTime.HasValue) ?
                        targetBook.Appointments.Max(a => a.AppointingDateTime) : null;

                    await _lendingInfoDbContext.SaveChangesAsync();
                    TempData["message"] = "取消成功";
                }
            }
            return RedirectToAction("PersonalInfo");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReBorrow(IEnumerable<string> barcodes)
        {
            StringBuilder borrowSuccess = new StringBuilder();
            StringBuilder borrowFail = new StringBuilder();
            borrowSuccess.Append("成功续借书籍:");
            borrowFail.Append("续借失败书籍：");
            foreach (var barcode in barcodes)
            {   
                Book reBorrowBook = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == barcode);
                if (reBorrowBook != null)
                {
                    if (reBorrowBook.State == BookState.Borrowed && DateTime.Now-reBorrowBook.MatureTime?.Date<=TimeSpan.FromDays(3))
                    {
                        reBorrowBook.State = BookState.ReBorrowed;
                        reBorrowBook.BorrowTime = DateTime.Now.Date;
                        reBorrowBook.MatureTime = DateTime.Now.Date+TimeSpan.FromDays(28);
                        borrowSuccess.Append($"《{reBorrowBook.Name}》、");
                    }
                    else
                    {
                        borrowFail.Append($"《{reBorrowBook.Name}》、");
                    }
                }
            }
            borrowSuccess.AppendLine(borrowFail.ToString());
            await _lendingInfoDbContext.SaveChangesAsync();
            TempData["message"] = borrowSuccess.ToString();
            return RedirectToAction("PersonalInfo");
        }

        [Authorize]
        public async Task<IActionResult> PersonalInfo()
        {
            StudentInfo student = await _lendingInfoDbContext.Students.Include(s => s.KeepingBooks).ThenInclude(k => k.Book)
                .FirstOrDefaultAsync(s => s.UserName == User.Identity.Name);
            decimal fine = 0;            
            foreach (var book in student.KeepingBooks.Where(b => b.Book.MatureTime < DateTime.Now && !b.AppointingDateTime.HasValue))
            {
                fine += (DateTime.Now - book.Book.MatureTime.Value).Days * (decimal)0.2;
                book.Book.State = book.Book.State == BookState.Appointed ? BookState.Appointed : BookState.Expired;
            }

            student.Fine = fine;
            PersonalInfoViewModel model = new PersonalInfoViewModel()
            {
                Student = student,
                BookingBook = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == student.AppointingBookBarCode)
            };
            return View(model);
        }

        public IActionResult Recommend()
        {
            RecommendedBook book=new RecommendedBook();            
            return View(book);
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
                books = _lendingInfoDbContext.BooksDetail.AsNoTracking();
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

                await _lendingInfoDbContext.BooksDetail.AddAsync(bookDetails);

                _lendingInfoDbContext.SaveChanges();
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
                BookDetails bookDetails = _lendingInfoDbContext.BooksDetail.First(b => b.ISBN == isbn);
                IQueryable<Book> books = _lendingInfoDbContext.Books.Where(b => b.ISBN == isbn);
                _lendingInfoDbContext.BooksDetail.Remove(bookDetails);
                _lendingInfoDbContext.Books.RemoveRange(books);
                sb.Append("《" + bookDetails.Name + "》");
                await _lendingInfoDbContext.SaveChangesAsync();
            }
            TempData["message"] = $"已移除书籍{sb.ToString()}";
            return RedirectToAction("BookDetails");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBookDetails(string isbn)
        {
            BookDetails book = await _lendingInfoDbContext.BooksDetail.FirstOrDefaultAsync(b => b.ISBN == isbn);
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
            BookDetails bookDetails = _lendingInfoDbContext.BooksDetail.FirstOrDefault(b => b.ISBN == model.ISBN);
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

                    await _lendingInfoDbContext.SaveChangesAsync();
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
                    bookDetails = await _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.Name == value);
                    break;
                case "ISBN":
                    bookDetails = await _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.ISBN == value);
                    break;
                case "FetchBookNumber":
                    bookDetails = await _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefaultAsync(b => b.FetchBookNumber == value);
                    break;
            }

            if (bookDetails != null)
            {
                if (User.Identity.IsAuthenticated&& User.IsInRole("Admin"))
                {
                    return RedirectToAction("EditBookDetails", new { isbn = bookDetails.ISBN });
                }
                else
                {
                    return RedirectToAction("Detail", new {isbn = bookDetails.ISBN});
                }                
            }

            TempData["message"] = "找不到该书籍";
            return RedirectToAction("BookDetails");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Books(string isbn)
        {
            BookEditModel model = new BookEditModel()
            {
                Books = _lendingInfoDbContext.Books.Include(b => b.Keeper).AsNoTracking().Where(b => b.ISBN == isbn),
                BookDetails = _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn)
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
            BookDetails bookDetails = _lendingInfoDbContext.BooksDetail.FirstOrDefault(b => b.ISBN == isbn);
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
                BookDetails bookDetails = _lendingInfoDbContext.BooksDetail.FirstOrDefault(b => b.ISBN == book.ISBN);
                Book existBook = _lendingInfoDbContext.Books.AsNoTracking().FirstOrDefault(b => b.BarCode == book.BarCode);
                if (existBook != null)
                {
                    TempData["message"] = $"已有二维码为{book.BarCode}的书籍《{existBook.Name}》";
                    return RedirectToAction("AddBook", new { isbn = book.ISBN });
                }
                if (bookDetails.Name == book.Name)
                {
                    Bookshelf bookshelf = _lendingInfoDbContext.Bookshelves.Include(b => b.Books).FirstOrDefault(b => b.BookshelfId == book.BookshelfId);
                    if (bookshelf != null)
                    {
                        book.Sort = bookshelf.Sort;
                        book.Location = bookshelf.Location;
                        bookshelf.Books.Add(book);
                        bookshelf.MaxFetchNumber = bookshelf.Books.Max(b => b.FetchBookNumber);
                        bookshelf.MinFetchNumber = bookshelf.Books.Min(b => b.FetchBookNumber);
                    }
                    await _lendingInfoDbContext.Books.AddAsync(book);
                    await _lendingInfoDbContext.SaveChangesAsync();
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
                Book book = _lendingInfoDbContext.Books.First(b => b.BarCode == barcode);
                _lendingInfoDbContext.Books.Remove(book);
                sb.AppendLine($"{book.BarCode} 移除成功");
            }
            await _lendingInfoDbContext.SaveChangesAsync();
            TempData["message"] = sb.ToString();
            return RedirectToAction("Books", new { isbn = isbn });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditBook(string barcode)
        {
            Book book = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == barcode);
            if (book == null)
            {
                return RedirectToAction("BookDetails");
            }
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(string oldBarCode, [Bind("BarCode,BookshelfId,Name,State")]Book book)
        {
            if (ModelState.IsValid)
            {
                Book oldBook = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == oldBarCode);
                if (oldBook == null)
                {
                    TempData["message"] = $"不存在二维码为{oldBarCode}的书籍";
                    return RedirectToAction("BookDetails");
                }

                if (oldBook.Name == book.Name)
                {
                    book.ISBN = oldBook.ISBN;
                    book.FetchBookNumber = oldBook.FetchBookNumber;
                    Bookshelf bookshelf = _lendingInfoDbContext.Bookshelves.Include(b => b.Books).FirstOrDefault(b => b.BookshelfId == book.BookshelfId);
                    if (bookshelf != null)
                    {
                        book.Sort = bookshelf.Sort;
                        book.Location = bookshelf.Location;
                        bookshelf.Books.Remove(oldBook);
                        bookshelf.Books.Add(book);
                    }

                    _lendingInfoDbContext.Books.Remove(oldBook);
                    _lendingInfoDbContext.Books.Add(book);
                    await _lendingInfoDbContext.SaveChangesAsync();
                    TempData["message"] = "修改成功";
                    return RedirectToAction("Books", new { isbn = oldBook.ISBN });
                }
            }
            return View(book);
        }

        [Authorize]
        public async Task<IActionResult> Lending(string barcode)
        {
            Book targetBook=await _lendingInfoDbContext.Books.Include(b=>b.Appointments).FirstOrDefaultAsync(b => b.BarCode == barcode);
            if (targetBook==null)
            {
                TempData["message"] = "请重新扫描书籍";
                return RedirectToAction("PersonalInfo");
            }

            if (targetBook.Appointments.Any(a=>a.AppointingDateTime.HasValue))
            {
                TempData["message"] = "此书已被预约";
                return RedirectToAction("PersonalInfo");
            }

            if (targetBook.State==BookState.Readonly)
            {
                TempData["message"] = "此书不供外借";
                return RedirectToAction("PersonalInfo");
            }

            targetBook.State = BookState.Borrowed;
            targetBook.BorrowTime = DateTime.Now.Date;
            targetBook.MatureTime = DateTime.Now.Date+TimeSpan.FromDays(28);
            StudentInfo student =
                await _lendingInfoDbContext.Students.Include(s=>s.KeepingBooks).FirstOrDefaultAsync(s => s.UserName == User.Identity.Name);
            student.KeepingBooks.Add(new AppointmentOrLending()
            {
                BookId = targetBook.BarCode,
                StudentId = student.UserName
            });
            await _lendingInfoDbContext.SaveChangesAsync();
            TempData["message"] = "借书成功";
            return RedirectToAction("PersonalInfo");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult EditLendingInfo(string barcode)
        {
            if (barcode == null)
            {
                return RedirectToAction("BookDetails");
            }
            Book book = _lendingInfoDbContext.Books.FirstOrDefault(b => b.BarCode == barcode);
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLendingInfo([Bind("BarCode,ISBN,BorrowTime,KeeperId,AppointedLatestTime,State")]Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.BorrowTime > DateTime.Now)
                {
                    ModelState.AddModelError("", "请检查外借时间");
                    return View(book);
                }
                if (book.AppointedLatestTime.HasValue)
                {
                    if (book.AppointedLatestTime < DateTime.Now)
                    {
                        ModelState.AddModelError("", "请检查预约时间");
                        return View(book);
                    }

                    if (book.KeeperId == null)
                    {
                        ModelState.AddModelError("", "不存在该学生");
                        return View(book);
                    }
                }

                StudentInfo student = await _lendingInfoDbContext.Students.Include(s => s.KeepingBooks).FirstOrDefaultAsync(s => s.UserName == book.KeeperId);

                Book addedBook = _lendingInfoDbContext.Books
                    .Include(b => b.Keeper).ThenInclude(k => k.KeepingBooks)
                    .FirstOrDefault(b => b.BarCode == book.BarCode);
                if (addedBook == null)
                {
                    return RedirectToAction("Books", new { isbn = book.ISBN });
                }

                StudentInfo preStudent = addedBook.Keeper;
                AppointmentOrLending targetLending =
                    preStudent?.KeepingBooks.FirstOrDefault(b => b.BookId == addedBook.BarCode);

                addedBook.AppointedLatestTime = book.AppointedLatestTime;
                addedBook.State = book.State;
                addedBook.BorrowTime = book.BorrowTime;
                addedBook.MatureTime = null;

                preStudent?.KeepingBooks.Remove(targetLending);

                if (addedBook.BorrowTime.HasValue)
                {
                    if (book.KeeperId == null)
                    {
                        ModelState.AddModelError("", "请检查借阅者");
                        return View(book);
                    }

                    if (student == null)
                    {
                        ModelState.AddModelError("", "不存在该学生");
                        return View(book);
                    }
                    if (student != null)
                    {
                        if (student.KeepingBooks.Count >= student.MaxBooksNumber)
                        {
                            TempData["message"] = "该学生借书已超过上限";
                        }

                        addedBook.State = BookState.Borrowed;
                        student.KeepingBooks.Add(new AppointmentOrLending()
                        {
                            BookId = addedBook.BarCode,
                            StudentId = student.UserName
                        });
                        addedBook.Keeper = student;

                    }
                    addedBook.MatureTime = addedBook.BorrowTime + TimeSpan.FromDays(28);
                }


                TempData["message"] = "保存成功";
                await _lendingInfoDbContext.SaveChangesAsync();
                return RedirectToAction("Books", new { isbn = book.ISBN });
            }
            return View(book);
        }


        public FileContentResult GetImage(string isbn)
        {
            BookDetails target = _lendingInfoDbContext.BooksDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn);
            if (target != null)
            {
                return File(target.ImageData, target.ImageMimeType);
            }
            return null;
        }
    }
}