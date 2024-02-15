using Microsoft.AspNetCore.Mvc;
using PractiseTest1.DTO;
using PractiseTest1.Repo;
using System;
using log4net;
using PractiseTest1.DTO;
using PractiseTest1.Repo;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace PractiseTest1.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private UnitOfWork unitOfWork;
        private ILog _logger; 

        public BookController(UnitOfWork uw, ILog logger)
        {
            unitOfWork = uw;
            _logger = logger;
        }

        [HttpGet,Route("GetAllBooks")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult GetAllBooks()
        {
            var books = unitOfWork.BookImplObject.GetAll();
            return Ok(books);
        }

        [HttpGet,Route("GetBookById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                var book = unitOfWork.BookImplObject.GetByBookId(id);

                if (book == null)
                {
                    return NotFound($"Book with ID {id} not found");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost,Route("AddBook")]
        [Authorize(Roles ="Admin")]
        public IActionResult AddBook(BookDTO newBook)
        {
            try
            {
                bool status = unitOfWork.BookImplObject.Add(newBook);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(newBook);
                }
                else
                {
                    return BadRequest("Error in adding book");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateBook/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBook(int id, BookDTO updatedBook)
        {
            updatedBook.BookId = id;

            try
            {
                bool status = unitOfWork.BookImplObject.Update(updatedBook);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(updatedBook);
                }
                else
                {
                    return BadRequest($"Book with ID {id} not found or update failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteBook/{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteBook(int id)
        {
            try
            {
                bool status = unitOfWork.BookImplObject.Delete(id);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok($"Book with ID {id} deleted");
                }
                else
                {
                    return BadRequest($"Book with ID {id} not found or deletion failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("SearchBooksByAuthor/{author}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SearchBooksByAuthor(string author)
        {
            try
            {
                var books = unitOfWork.BookImplObject.SearchBooksByAuthor(author);

                if (books == null || books.Count == 0)
                {
                    return NotFound($"No books found for author: {author}");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("SearchBooksByGenre/{genre}")]
        [Authorize(Roles ="User")]
        public IActionResult SearchBooksByGenre(string genre)
        {
            try
            {
                var books = unitOfWork.BookImplObject.SearchBooksByGenre(genre);

                if (books == null || books.Count == 0)
                {
                    return NotFound($"No books found for genre: {genre}");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("SearchBooksByTitle/{title}")]
        [AllowAnonymous] 
        public IActionResult SearchBooksByTitle(string title)
        {
            try
            {
                var books = unitOfWork.BookImplObject.SearchBooksByTitle(title);

                if (books == null || books.Count == 0)
                {
                    return NotFound($"No books found for title: {title}");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

       

    }
}
