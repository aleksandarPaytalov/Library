using System.Security.Claims;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {

        private readonly LibraryDbContext data;

        public BookController(LibraryDbContext context)
        {
            data = context;
        }

        public async Task<IActionResult> All()
        {
            var books = await data.Books
                .AsNoTracking()
                .Select(b => new AllBooksViewModel()
                {
                    Id = b.Id,
                    Author = b.Author,
                    Category = b.Category.Name,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Title = b.Title
                })
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Mine()
        {
            string userId = GetUserId();

            var model = await data.IdentityUsersBooks
                .Where(ub => ub.CollectorId == userId)
                .Select(ub => new AllBooksViewModel()
                {
                    Id = ub.Book.Id,
                    Author = ub.Book.Author,
                    Category = ub.Book.Category.Name,
                    Description = ub.Book.Description,
                    ImageUrl = ub.Book.ImageUrl,
                    Rating = ub.Book.Rating,
                    Title = ub.Book.Title
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> AddToCollection(int id)
        {
            var book = await data.Books
                .Where(b => b.Id == id)
                .Select(b => new AllBooksViewModel()
                {
                    Id = b.Id,
                    Author = b.Author,
                    Category = b.Category.Name,
                    Description = b.Description,
                    Rating = b.Rating,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl
                })
                .FirstOrDefaultAsync();
        
            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }
        
            var userId = GetUserId();
        
            var alreadyAdded = await data.IdentityUsersBooks
                .FirstOrDefaultAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);
            if (alreadyAdded == null)
            {
                var userBook = new IdentityUserBook()
                {
                    CollectorId = userId,
                    BookId = book.Id
                };
        
                await data.IdentityUsersBooks.AddAsync(userBook);
                await data.SaveChangesAsync();
            }
        
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await data.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            AddBookViewModel model = new AddBookViewModel()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newBook = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.Url,
                Rating = model.Rating,
                CategoryId = model.CategoryId
            };

            await data.Books.AddAsync(newBook);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var book = await data.Books
                .Where(b => b.Id == id)
                .Select(b => new AllBooksViewModel()
                {
                    Id = b.Id,
                    Author = b.Author,
                    Category = b.Category.Name,
                    Description = b.Description,
                    Rating = b.Rating,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return RedirectToAction(nameof(Mine));
            }

            string userId = GetUserId();
            var bookToRemove = await data.IdentityUsersBooks
                .FirstOrDefaultAsync(ub => 
                    ub.CollectorId == userId && 
                    ub.BookId == book.Id);

            if (bookToRemove != null)
            {
                data.IdentityUsersBooks.Remove(bookToRemove);
                await data.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Mine));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await data.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync(); ;

            var book = await data.Books
                .Where(b => b.Id == id)
                .Select(b => new AddBookViewModel
                {
                    Author = b.Author,
                    Description = b.Description,
                    Rating = b.Rating,
                    Title = b.Title,
                    Url = b.ImageUrl,
                    CategoryId = b.CategoryId,
                    Categories = categories
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }

            return View(book);
        }

        public async Task<IActionResult> Edit(AddBookViewModel model, int id)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var book = await data.Books.FindAsync(id);

            if (book != null)
            {
                book.Title = model.Title;
                book.Author = model.Author;
                book.ImageUrl = model.Url;
                book.Rating = model.Rating;
                book.Description = model.Description;
                book.CategoryId = model.CategoryId;

                await data.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await data.Books
                .FindAsync(id);

            if (book != null)
            {
                data.Books.Remove(book);
                await data.SaveChangesAsync();
            }

            return RedirectToAction(nameof(All));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
