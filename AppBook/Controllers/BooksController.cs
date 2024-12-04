using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NumePrenume.Data;
using NumePrenume.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NumePrenume.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Denumire,Autor,Editura,Pret")] Book book, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                    var extension = Path.GetExtension(Image.FileName);
                    var newFileName = fileName + "_" + System.Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    book.ImagePath = "/images/" + newFileName;
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Denumire,Autor,Editura,Pret,ImagePath")] Book book, IFormFile Image)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                        var extension = Path.GetExtension(Image.FileName);
                        var newFileName = fileName + "_" + System.Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        book.ImagePath = "/images/" + newFileName;
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.ID == book.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        var dbBook = await _context.Books.FindAsync(book.ID);
                        if (dbBook == null)
                        {
                            return NotFound();
                        }

                        dbBook.Denumire = book.Denumire;
                        dbBook.Autor = book.Autor;
                        dbBook.Editura = book.Editura;
                        dbBook.Pret = book.Pret;
                        dbBook.ImagePath = book.ImagePath;

                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
