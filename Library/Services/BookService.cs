using Library.Contracts;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _dbContext;

        public BookService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<AllBooksViewModel>> GetAllBooksAsync()
        {
            return await _dbContext.Books
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
        }

        public async Task<ICollection<AllBooksViewModel>> GetMyBooksAsync(string userId)
        {
            var model = await _dbContext.IdentityUsersBooks
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

            return (model);
        }

        public async Task<AllBooksViewModel?> GetBookByIdAsync(int id)
        {
            var book = await _dbContext.Books
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

            return book;
        }

        public async Task AddBookToMyCollectionAsync(AllBooksViewModel book, string userId)
        {
            var alreadyAdded = await _dbContext.IdentityUsersBooks
                .FirstOrDefaultAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);
            if (alreadyAdded == null)
            {
                var userBook = new IdentityUserBook()
                {
                    CollectorId = userId,
                    BookId = book.Id
                };
            
                await _dbContext.IdentityUsersBooks.AddAsync(userBook);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<AddBookViewModel> AddNewBookAsyncGet()
        {
            var categories = await _dbContext.Categories
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

            return model;
        }

        public async Task AddNewBookAsyncPost(AddBookViewModel model)
        {
            var newBook = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.Url,
                Rating = model.Rating,
                CategoryId = model.CategoryId
            };

            await _dbContext.Books.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AddBookViewModel?> GetBookByIdForEditAsync(int id)
        {
            var categories = await _dbContext.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync(); ;

            var book = await _dbContext.Books
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

            return book;
        }

        public async Task EditBookAsync(AddBookViewModel model, int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book != null)
            {
                book.Title = model.Title;
                book.Author = model.Author;
                book.ImageUrl = model.Url;
                book.Rating = model.Rating;
                book.Description = model.Description;
                book.CategoryId = model.CategoryId;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task BookToRemoveFromCollectionAsync(int bookId, string userId)
        {
            var bookToRemove = await _dbContext.IdentityUsersBooks
                .FirstOrDefaultAsync(ub => 
                    ub.CollectorId == userId && 
                    ub.BookId == bookId);
            
            if (bookToRemove != null)
            {
                _dbContext.IdentityUsersBooks.Remove(bookToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _dbContext.Books
                .FindAsync(id);
            
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
