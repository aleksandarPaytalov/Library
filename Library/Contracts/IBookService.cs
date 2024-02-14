using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task<ICollection<AllBooksViewModel>> GetAllBooksAsync();
        Task<ICollection<AllBooksViewModel>> GetMyBooksAsync(string userId);
        Task<AllBooksViewModel?> GetBookByIdAsync(int id);
        Task AddBookToMyCollectionAsync(AllBooksViewModel book, string userId);
        Task<AddBookViewModel> AddNewBookAsyncGet();
        Task AddNewBookAsyncPost(AddBookViewModel model);
        Task<AddBookViewModel?> GetBookByIdForEditAsync(int id);
        Task EditBookAsync(AddBookViewModel model, int id);
        Task BookToRemoveFromCollectionAsync(int bookId, string userId);
        Task DeleteBookAsync(int id);
    }
}
