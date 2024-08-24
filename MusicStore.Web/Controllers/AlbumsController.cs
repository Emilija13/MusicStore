using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Domain.Domain;
using MusicStore.Repository;
using MusicStore.Service.Implementation;
using MusicStore.Service.Interface;

namespace MusicStore.Web.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
       

        public AlbumsController(IArtistService artistService, IAlbumService albumService)
        {
            _artistService = artistService;
            _albumService = albumService;
        }

        // GET: Albums
        public IActionResult Index()
        {
            return View(_albumService.GetAllAlbums());
        }

        // GET: Albums/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _albumService.GetDetailsForAlbum(id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            var artists = _artistService.GetAllArtists();

            ViewBag.ArtistId = new SelectList(artists, "Id", "Name");

            return View();

            //ViewData["ArtistId"] = new SelectList(_artistService.GetAllArtists(), "Id", "ArtistImage");
            //return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AlbumName,ReleaseDate,AlbumImage,Rating,ArtistId,Id")] Album album)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                album.Id = Guid.NewGuid();
                _albumService.CreateNewAlbum(album);
                return RedirectToAction(nameof(Index));
            }
            var artists = _artistService.GetAllArtists();
            ViewBag.ArtistId = new SelectList(artists, "Id", "Name");
            return View(album);
        }

        // GET: Albums/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _albumService.GetDetailsForAlbum(id);
            if (album == null)
            {
                return NotFound();
            }
            var artists = _artistService.GetAllArtists();
            ViewBag.ArtistId = new SelectList(artists, "Id", "Name");
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("AlbumName,ReleaseDate,AlbumImage,Rating,ArtistId,Id")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _albumService.UpdateExistingAlbum(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var artists = _artistService.GetAllArtists();
            ViewBag.ArtistId = new SelectList(artists, "Id", "Name");
            return View(album);
        }

        // GET: Albums/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _albumService?.GetDetailsForAlbum(id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _albumService.DeleteAlbum(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(Guid id)
        {
            return _albumService.GetAllAlbums().Any(e => e.Id == id);
        }
    }
}
