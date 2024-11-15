using LibraryService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.WebAPI.Services
{
    public class BooksService : IBooksService
    {
        private readonly LibraryContext _libraryContext;
        private readonly IBooksService _booksService;
        public BooksService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task<IEnumerable<Book>> Get(int libraryId, int[] ids)
        {
            // Complete the implementation
            var books =  await _libraryContext.Books.Where(b => b.LibraryId == libraryId).ToListAsync();
            books = books.Where(b => ids.Contains(b.Id)).ToList();
            return books;
        }

        public async Task<IEnumerable<Book>> GetAll(int libraryId)
        {
            // Complete the implementation
            var x = await _libraryContext.Books.ToListAsync();
            var books =  await _libraryContext.Books.Where(b => b.LibraryId == libraryId).ToListAsync();
            return books;
        }

        public async Task<Book> Add(Book book)
        {
            // Complete the implementation
            var b = _libraryContext.Books.SingleOrDefault(b => b.Id == book.Id);
            if (b != null)
                return book;

            await _libraryContext.Books.AddAsync(book);

            await _libraryContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book> Update(Book book)
        {
            // Complete the implementation
            var projectForChanges = await _libraryContext.Books.SingleAsync(x => x.Id == book.Id);
            projectForChanges.Name = book.Name;
            projectForChanges.Category = book.Category;
            projectForChanges.LibraryId = book.LibraryId;

            _libraryContext.Books.Update(projectForChanges);
            await _libraryContext.SaveChangesAsync();
            return book;
        }

        public async Task<bool> Delete(Book book)
        {
            // Complete the implementation
            var lib = await _libraryContext.Books.SingleOrDefaultAsync(l => l.Id == book.Id);
            if (lib == null)
                return false;
            _libraryContext.Books.Remove(book);
            await _libraryContext.SaveChangesAsync();
            return true;
        }
    }

    public interface IBooksService
    {
        Task<IEnumerable<Book>> Get(int libraryId, int[] ids);

        Task<Book> Add(Book book);

        Task<Book> Update(Book book);

        Task<bool> Delete(Book book);
        Task<IEnumerable<Book>> GetAll(int libraryId);
    }
}
