using PractiseTest1.DTO;
using PractiseTest1.Entities;
namespace PractiseTest1.Repo
{
    public class BookImpl:IRepo<BookDTO>
    {
        private readonly BookDbContext _context;

        public BookImpl(BookDbContext context)
        {
            _context = context;
        }

        public bool Add(BookDTO item)
        {
            Book bookNew = new Book
            {
                Title = item.Title,
                Author = item.Author,
                Genre = item.Genre,
                ISBN = item.ISBN,
                PublishDate = item.PublishDate
            };

            _context.Books.Add(bookNew);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int bookId)
        {
            var bookDelete = _context.Books.Find(bookId);

            if (bookDelete == null)
                return false;

            _context.Books.Remove(bookDelete);
            _context.SaveChanges();
            return true;
        }

        public List<BookDTO> GetAll()
        {
            var result = _context.Books
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    PublishDate = b.PublishDate
                })
                .ToList();
            return result;
        }

        public bool Update(BookDTO item)
        {
            var bookUpdate = _context.Books.Find(item.BookId);

            if (bookUpdate == null)
                return false;

            bookUpdate.Title = item.Title;
            bookUpdate.Author = item.Author;
            bookUpdate.Genre = item.Genre;
            bookUpdate.ISBN = item.ISBN;
            bookUpdate.PublishDate = item.PublishDate;

            _context.SaveChanges();

            return true;
        }

        public BookDTO GetByBookId(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);

            if (book == null)
                return null;

            var bookDTO = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ISBN = book.ISBN,
                PublishDate = book.PublishDate
            };

            return bookDTO;
        }
    }
}
