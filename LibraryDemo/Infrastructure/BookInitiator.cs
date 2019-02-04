using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Data;
using LibraryDemo.Models.DomainModels;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDemo.Infrastructure
{
    public class BookInitiator
    {
        public static async Task BookInitial(IServiceProvider serviceProvider)
        {
            LendingInfoDbContext context = serviceProvider.GetRequiredService<LendingInfoDbContext>();
            if (!context.Books.Any() || !context.Bookshelves.Any())
            {
                Bookshelf[] bookshelfs = new[]
                {
                    new Bookshelf()
                    {
                        BookshelfId = 1,
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Bookshelf()
                    {
                        BookshelfId = 2,
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Bookshelf()
                    {
                        BookshelfId = 3,
                        Location = "东校区",
                        Sort = "计算机"
                    },
                    new Bookshelf()
                    {
                        BookshelfId = 4,
                        Location = "阅览室",
                        Sort = "文学"
                    },
                    new Bookshelf()
                    {
                        BookshelfId = 5,
                        Location = "阅览室",
                        Sort = "计算机"
                    },
                };

                Book[] books = new[]
                {
                    new Book()
                    {
                        Name = "精通ASP.NET MVC 5",
                        BarCode = "001100987211",
                        ISBN = "978-7-115-41023-8",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.092 19",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "精通ASP.NET MVC 5",
                        BarCode = "001100987212",
                        ISBN = "978-7-115-41023-8",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.092 19",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "精通ASP.NET MVC 5",
                        BarCode = "001100987213",
                        ISBN = "978-7-115-41023-8",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.092 19",
                        Location = "东校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "精通ASP.NET MVC 5",
                        BarCode = "001100987214",
                        ISBN = "978-7-115-41023-8",
                        State = BookState.Readonly,
                        FetchBookNumber = "TP393.092 19",
                        Location = "阅览室",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Entity Framework实用精要",
                        BarCode = "001101279682",
                        ISBN = "978-7-302-48593-3",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 447",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Entity Framework实用精要",
                        BarCode = "001101279683",
                        ISBN = "978-7-302-48593-3",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 447",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Entity Framework实用精要",
                        BarCode = "001101279684",
                        ISBN = "978-7-302-48593-3",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 447",
                        Location = "东校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Entity Framework实用精要",
                        BarCode = "001101279685",
                        ISBN = "978-7-302-48593-3",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 447",
                        Location = "东校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Entity Framework实用精要",
                        BarCode = "001101279686",
                        ISBN = "978-7-302-48593-3",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 447",
                        Location = "阅览室",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Rails 5敏捷开发",
                        BarCode = "001101290497",
                        ISBN = "978-7-5680-3659-7",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 448",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Rails 5敏捷开发",
                        BarCode = "001101290498",
                        ISBN = "978-7-5680-3659-7",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 448",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "Rails 5敏捷开发",
                        BarCode = "001101290499",
                        ISBN = "978-7-5680-3659-7",
                        State = BookState.Readonly,
                        FetchBookNumber = "TP393.09 448",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "你必须掌握的Entity Framework 6.x与Core 2.0",
                        BarCode = "001101362986",
                        ISBN = "978-7-5680-3659-7",
                        State = BookState.Normal,
                        FetchBookNumber = "TP393.09 452",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "你必须掌握的Entity Framework 6.x与Core 2.0",
                        BarCode = "001101362987",
                        ISBN = "978-7-5680-3659-7",
                        State = BookState.Readonly,
                        FetchBookNumber = "TP393.09 452",
                        Location = "主校区",
                        Sort = "计算机"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第一卷",
                        BarCode = "00929264",
                        ISBN = "7-01-000922-8",
                        State = BookState.Normal,
                        FetchBookNumber = "A41 1:1",
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第一卷",
                        BarCode = "00929265",
                        ISBN = "7-01-000922-8",
                        State = BookState.Normal,
                        FetchBookNumber = "A41 1:1",
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第一卷",
                        BarCode = "00929266",
                        ISBN = "7-01-000922-8",
                        State = BookState.Readonly,
                        FetchBookNumber = "A41 1:1",
                        Location = "阅览室",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第二卷",
                        BarCode = "00929279",
                        ISBN = "7-01-000915-5",
                        State = BookState.Normal,
                        FetchBookNumber = "A41 1:2",
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第二卷",
                        BarCode = "00929280",
                        ISBN = "7-01-000915-5",
                        State = BookState.Readonly,
                        FetchBookNumber = "A41 1:2",
                        Location = "阅览室",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第三卷",
                        BarCode = "00930420",
                        ISBN = "7-01-000916-3",
                        State = BookState.Normal,
                        FetchBookNumber = "A41 1:3",
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第三卷",
                        BarCode = "00930421",
                        ISBN = "7-01-000916-3",
                        State = BookState.Readonly,
                        FetchBookNumber = "A41 1:3",
                        Location = "阅览室",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第四卷",
                        BarCode = "00930465",
                        ISBN = "7-01-000925-2",
                        State = BookState.Normal,
                        FetchBookNumber = "A41 1:4",
                        Location = "主校区",
                        Sort = "文学"
                    },
                    new Book()
                    {
                        Name = "毛泽东选集. 第四卷",
                        BarCode = "00930466",
                        ISBN = "7-01-000925-2",
                        State = BookState.Readonly,
                        FetchBookNumber = "A41 1:4",
                        Location = "阅览室",
                        Sort = "文学"
                    }
                };

                BookDetails[] bookDetails = new[]
                {
                    new BookDetails()
                    {
                        Author = "Admam Freeman",
                        Name = "精通ASP.NET MVC 5",
                        ISBN = "978-7-115-41023-8",
                        Press = "人民邮电出版社",
                        PublishDateTime = new DateTime(2016,1,1),
                        SoundCassettes = "13, 642页 : 图 ; 24cm",
                        Version = 1,
                        FetchBookNumber = "TP393.092 19",
                        Description = "ASP.NET MVC 5框架是微软ASP.NET Web平台的新进展。它提供了高生产率的编程模型，结合ASP.NET的全部优势，促成更整洁的代码架构、测试驱动开发和强大的可扩展性。本书涵盖ASP.NET MVC 5的所有开发优势技术，包括用C#属性定义路由技术及重写过滤器技术等。且构建MVC应用程序的用户体验也有本质上的改进。其中书里也专一讲解了用新Visual Studio 2013创建MVC应用程序时的技术和技巧。本书包括完整的开发工具介绍以及对代码进行辅助编译和调试的技术。本书还涉及流行的Bootstrap JavaScript库，该库现已被纳入到MVC 5之中，为开发人员提供更广泛的多平台CSS和HTML5选项，而不必像以前那样去加载大量的第三方库。"
                    },
                    new BookDetails()
                    {
                        Author = "吕高旭",
                        Name = "Entity Framework实用精要",
                        ISBN = "978-7-302-48593-3",
                        Press = "清华大学出版社",
                        PublishDateTime = new DateTime(2018,1,1),
                        SoundCassettes = "346页 ; 26cm",
                        Version = 1,
                        FetchBookNumber = "TP393.09 447",
                        Description = "本书通过介绍Entity Framework与 LINQ 开发实战的案例，以 Entity Framework 技术内容的讨论为主线，结合关键的 LINQ技巧说明，提供读者系统性学习 Entity Framework 所需的内容。本书旨在帮助读者进入 Entity Framework的世界，建立必要的技术能力，同时希望读者在完成本书的教学课程之后，能够更进一步地将其运用在实际的项目开发中。"
                    },
                    new BookDetails()
                    {
                        Author = "鲁比",
                        Name = "Rails 5敏捷开发",
                        ISBN = "978-7-5680-3659-7",
                        Press = "华中科技大学出版社",
                        PublishDateTime = new DateTime(2018,1,1),
                        SoundCassettes = "xxi, 451页 : 图 ; 23cm",
                        Version = 1,
                        FetchBookNumber = "TP393.09 448",
                        Description = "本书以讲解“购书网站”案例为主线, 逐步介绍Rails的内置功能。全书分为3部分, 第一部分介绍Rails的安装、应用程序验证、Rails框架的体系结构, 以及Ruby语言知识; 第二部分用迭代方式构建应用程序, 然后依据敏捷开发模式开展测试, 最后用Capistrano完成部署; 第三部分补充日常实用的开发知识。本书既有直观的示例, 又有深入的分析, 同时涵盖了Web开发各方面的知识, 堪称一部内容全面而又深入浅出的佳作。第5版增加了关于Rails 5和Ruby 2.2新特性和最佳实践的内容。"
                    },
                    new BookDetails()
                    {
                        Author = "汪鹏",
                        Name = "你必须掌握的Entity Framework 6.x与Core 2.0",
                        ISBN = "978-7-302-50017-9",
                        Press = "清华大学出版社",
                        PublishDateTime = new DateTime(2018,1,1),
                        SoundCassettes = "X, 487页 : 图 ; 26cm",
                        Version = 1,
                        FetchBookNumber = "TP393.09 452",
                        Description = "本书分为四篇，第一篇讲解Entity Framework 6.x的基础，包括数据库表的创建，数据的操作和数据加载方式。第二篇讲解Entity Framework 6.x进阶，包括基本原理和性能优化。第三篇讲解跨平台Entity Framework Core 2.x的基础知识和开发技巧。第四篇讲解在Entity Framework Core 2.x中解决并发问题，并给出实战开发案例。"
                    },
                    new BookDetails()
                    {
                        Author = "毛泽东",
                        Name = "毛泽东选集. 第一卷",
                        ISBN = "7-01-000922-8",
                        Press = "人民出版社",
                        PublishDateTime = new DateTime(1991,1,1),
                        SoundCassettes = "340页 : 肖像 ; 19厘米",
                        FetchBookNumber = "A41 1:1",
                        Description = "《毛泽东选集》是毛泽东思想的重要载体，是毛泽东思想的集中展现，是对20世纪中国影响最大的书籍之一。",
                        Version = 2
                    },
                    new BookDetails()
                    {
                        Author = "毛泽东",
                        Name = "毛泽东选集. 第二卷",
                        ISBN = "7-01-000915-5",
                        Press = "人民出版社",
                        PublishDateTime = new DateTime(1991,1,1),
                        SoundCassettes = "343-786页 : 肖像 ; 19厘米",
                        FetchBookNumber = "A41 1:2",
                        Description = "《毛泽东选集》是毛泽东思想的重要载体，是毛泽东思想的集中展现，是对20世纪中国影响最大的书籍之一。",
                        Version = 2
                    },
                    new BookDetails()
                    {
                        Author = "毛泽东",
                        Name = "毛泽东选集. 第三卷",
                        ISBN = "7-01-000916-3",
                        Press = "人民出版社",
                        PublishDateTime = new DateTime(1991,1,1),
                        SoundCassettes = "789Ł±1120页 ; 20厘米",
                        FetchBookNumber = "A41 1:3",
                        Description = "《毛泽东选集》是毛泽东思想的重要载体，是毛泽东思想的集中展现，是对20世纪中国影响最大的书籍之一。",
                        Version = 2
                    },
                    new BookDetails()
                    {
                        Author = "毛泽东",
                        Name = "毛泽东选集. 第四卷",
                        ISBN = "7-01-000925-2",
                        Press = "人民出版社",
                        PublishDateTime = new DateTime(1991,1,1),
                        SoundCassettes = "1123Ł±1517页 ; 20厘米",
                        FetchBookNumber = "A41 1:4",
                        Description = "《毛泽东选集》是毛泽东思想的重要载体，是毛泽东思想的集中展现，是对20世纪中国影响最大的书籍之一。",
                        Version = 2
                    },
                };

                var temp = from book in books
                           from bookshelf in bookshelfs
                           where book.Location == bookshelf.Location && book.Sort == bookshelf.Sort
                           select new { BarCode = book.BarCode, BookshelfId = bookshelf.BookshelfId };

                foreach (var bookshelf in bookshelfs)
                {
                    bookshelf.Books=new List<Book>();
                }

                foreach (var tem in temp)
                {
                    Bookshelf targetShelf = bookshelfs.Single(bookshelf => bookshelf.BookshelfId == tem.BookshelfId);
                    Book targetBook = books.Single(book => book.BarCode == tem.BarCode);
                    targetShelf.Books.Add(targetBook);
                }

                foreach (var bookshelf in bookshelfs)
                {
                    bookshelf.MaxFetchNumber=bookshelf.Books.Max(b => b.FetchBookNumber);
                    bookshelf.MinFetchNumber=bookshelf.Books.Min(b => b.FetchBookNumber);
                }

                foreach (var bookshelf in bookshelfs)
                {
                    await context.Bookshelves.AddAsync(bookshelf);
                    await context.SaveChangesAsync();
                }

                foreach (var bookDetail in bookDetails)
                {
                    await context.BooksDetail.AddAsync(bookDetail);
                    await context.SaveChangesAsync();
                }

                foreach (var book in books)
                {
                    await context.Books.AddAsync(book);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Students.Any())
            {
                IEnumerable<StudentInfo> initialStudents = new[]
                {
                    new StudentInfo()
                    {
                        UserName = "U201600001",
                        Name = "Nanase",
                        PhoneNumber = "12345678910",
                        Degree = Degrees.CollegeStudent,
                        MaxBooksNumber = 10,
                    },
                    new StudentInfo()
                    {
                        UserName = "U201600002",
                        Name = "Ruri",
                        PhoneNumber = "12345678911",
                        Degree = Degrees.DoctorateDegree,
                        MaxBooksNumber = 15
                    }
                };

                IEnumerable<StudentInfo> initialAdmins = new[]
                {
                    new StudentInfo()
                    {
                        UserName = "A000000000",
                        Name="Admin0000",
                        PhoneNumber = "12345678912",
                        Degree = Degrees.CollegeStudent,
                        MaxBooksNumber = 20
                    },
                    new StudentInfo()
                    {
                        UserName = "A000000001",
                        Name = "Admin0001",
                        PhoneNumber = "15827411963",
                        Degree = Degrees.CollegeStudent,
                        MaxBooksNumber = 20
                    },
                };
                foreach (var student in initialStudents)
                {
                    context.Students.Add(student);
                    await context.SaveChangesAsync();
                }
                foreach (var admin in initialAdmins)
                {
                    context.Students.Add(admin);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
