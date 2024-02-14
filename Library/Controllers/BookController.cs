using System.Security.Claims;
using Library.Contracts;
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
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }


        public async Task<IActionResult> All()
        {
            // put the code from bookService.GetAllBooksAsync() here if you don not want to have Services and Contracts
            var books = await bookService.GetAllBooksAsync();

            return View(books);
        }

        public async Task<IActionResult> Mine()
        {
            var userId = GetUserId();

            var model = await bookService.GetMyBooksAsync(userId);
        
            return View(model);
        }
        
        public async Task<IActionResult> AddToCollection(int id)
        {
            var book = await bookService.GetBookByIdAsync(id);
            
            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }
        
            var userId = GetUserId();

            await bookService.AddBookToMyCollectionAsync(book, userId);
        
            return RedirectToAction(nameof(All));
        }
        
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await bookService.AddNewBookAsyncGet();
        
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await bookService.AddNewBookAsyncPost(model);
        
            return RedirectToAction(nameof(All));
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var book = await bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return RedirectToAction(nameof(Mine));
            }

            string userId = GetUserId();
            await bookService.BookToRemoveFromCollectionAsync(book.Id, userId);

            return RedirectToAction(nameof(Mine));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var bookForEdit = await bookService.GetBookByIdForEditAsync(id);
            
        
            if (bookForEdit == null)
            {
                return RedirectToAction(nameof(All));
            }
        
            return View(bookForEdit);
        }
        
        public async Task<IActionResult> Edit(AddBookViewModel model, int id)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await bookService.EditBookAsync(model, id);
            
            return RedirectToAction(nameof(All));
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await bookService.DeleteBookAsync(id);
            
        
            return RedirectToAction(nameof(All));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
