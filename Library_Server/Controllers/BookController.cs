using Library_Server.Models;
using Library_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly BookService _bookService;

        public BookController(ILogger<BookController> logger, BookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            try
            {
                _logger.LogInformation("Start: BookController/GetBooks");
                var books = await _bookService.GetBooks();
                _logger.LogInformation("End: BookController/GetBooks");

                return Ok(books);
            } 
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }
        }
    }
}
