using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtProject.Data;
using ArtProject.Models;

namespace ArtProject.Controllers
{
    public class ArtsController : Controller
    {
        private readonly ArtProjectContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArtsController(ArtProjectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        // GET: Arts
        public async Task<IActionResult> Index(string searchString)
        {
            var arts = from a in _context.Art
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                arts = arts.Where(s => s.Title!.Contains(searchString) || s.Artist!.Contains(searchString) || s.Technique!.Contains(searchString) || s.Movement!.Contains(searchString));
            }
            return View(await arts.ToListAsync());
            //return _context.Art != null ? 
            //View(await _context.Art.ToListAsync()) :
            //Problem("Entity set 'ArtProjectContext.Art'  is null.");
        }

        // GET: Arts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Art == null)
            {
                return NotFound();
            }

            var art = await _context.Art
                .FirstOrDefaultAsync(m => m.Id == id);
            if (art == null)
            {
                return NotFound();
            }

            return View(art);
        }

        // GET: Arts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Arts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Artist,Date,Technique,Price,Movement,History,ImageFile")] Art art)
        {
            if (ModelState.IsValid)
            {
                //Save Image to wwwroot/images
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(art.ImageFile.FileName);
                string extension = Path.GetExtension(art.ImageFile.FileName);
                art.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await art.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(art);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(art);
        }

        // GET: Arts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Art == null)
            {
                return NotFound();
            }

            var art = await _context.Art.FindAsync(id);
            if (art == null)
            {
                return NotFound();
            }
            return View(art);
        }

        // POST: Arts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Artist,Date,Technique,Price,Movement,History,ImageFile")] Art art)
        {
            if (id != art.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(art);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtExists(art.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(art);
        }

        // GET: Arts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Art == null)
            {
                return NotFound();
            }

            var art = await _context.Art
                .FirstOrDefaultAsync(m => m.Id == id);
            if (art == null)
            {
                return NotFound();
            }

            return View(art);
        }

        // POST: Arts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Art == null)
            {
                return Problem("Entity set 'ArtProjectContext.Art'  is null.");
            }
            var art = await _context.Art.FindAsync(id);

            //Delete Image from wwwwroot/images
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", art.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            if (art != null)
            {
                _context.Art.Remove(art);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Arts
        public async Task<IActionResult> Gallery()
        {
            return _context.Art != null ? 
            View(await _context.Art.ToListAsync()) :
            Problem("Entity set 'ArtProjectContext.Art'  is null.");
        }

        private bool ArtExists(int id)
        {
          return (_context.Art?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
