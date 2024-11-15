
using Microsoft.AspNetCore.Mvc;
using LibraryService.WebAPI.Data;
using LibraryService.WebAPI.Services;

namespace LibraryService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrariesController : ControllerBase
    {
        private readonly ILibrariesService _librariesService;
        private readonly IBooksService _booksService;

        public LibrariesController(IBooksService booksService, ILibrariesService librariesService)
        {
            _librariesService = librariesService;
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libraries = await _librariesService.Get(null);
            return Ok(libraries);
        }

        [HttpGet("{libraryId}")]
        public async Task<IActionResult> Get(int libraryId)
        {
            var library = (await _librariesService.Get(new[] { libraryId })).FirstOrDefault();
            if (library == null)
            {
                return NotFound();
            }
            return Ok(library);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Library l)
        {
            await _librariesService.Add(l);

            var response = Content("created");
            response.StatusCode = 201;
            return response;
        }

        [HttpPut("{libraryId}")]
        public async Task<IActionResult> Update(int libraryId, Library library)
        {
            var existingLibrary = (await _librariesService.Get(new[] { libraryId })).FirstOrDefault();
            if (existingLibrary == null)
            {
                return NotFound();
            }

            await _librariesService.Update(library);
            return NoContent();
        }

        // Implement the DELETE method below
        [HttpDelete("{libraryId}")]
        public async Task<IActionResult> Delete(int libraryId)
        {
            var lib = await _librariesService.Get(new[] {libraryId});
            if (lib.ToList().Count == 0)
            {
                return NotFound();
            }

            var books = await _booksService.GetAll(libraryId);
            if (books.ToList().Count > 0)
            {
                foreach(Book book in books.ToList())
                {
                    await _booksService.Delete(book);
                }
            }
            var res = await _librariesService.Delete(lib.First());

            if (res)
                return NoContent();
            return 
                NotFound();
        }
    }
}
