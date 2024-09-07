using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly ITrackService _trackService;
        private readonly IUserPlaylistService _userPlaylistService;
       

        public AlbumsController(IArtistService artistService, IAlbumService albumService, ITrackService trackService, IUserPlaylistService userPlaylistService)
        {
            _artistService = artistService;
            _albumService = albumService;
            _trackService = trackService;
            _userPlaylistService = userPlaylistService;
        }

        // GET: Albums
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure that ViewBag.LoggedIn is always a boolean
            ViewBag.LoggedIn = userId != null;
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

            var totalDuration = _trackService.CalculateTotalDuration(id.Value);

            string formattedDuration;
            if (totalDuration[0] > 0)
            {
                formattedDuration = $"{totalDuration[0]} hr {totalDuration[1]} min";
            }
            else
            {
                formattedDuration = $"{totalDuration[1]} min {totalDuration[2]} sec";
            }

            ViewBag.TotalDuration = formattedDuration;

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
                return RedirectToAction("Details", new { id = album.Id });
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

        [HttpGet]
        public IActionResult AddTrack(Guid albumId)
        {
            var album = _albumService.GetDetailsForAlbum(albumId); // Retrieve album to associate with the track
            if (album == null)
            {
                return NotFound();
            }

            var track = new Track
            {
                AlbumId = albumId
            };

            return View(track);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTrack(Track track)
        {
            if (ModelState.IsValid)
            {
                _trackService.CreateNewTrack(track);
                return RedirectToAction("Edit", new { id = track.AlbumId });
            }
            return View(track);
        }

        public IActionResult EditTrack(Guid id)
        {
            var track = _trackService.GetDetailsForTrack(id);
            if (track == null)
            {
                return NotFound();
            }
            return View(track);
        }

        [HttpPost]
        public IActionResult EditTrack(Track track)
        {
            if (ModelState.IsValid)
            {
                _trackService.UpdateExistingTrack(track);
                return RedirectToAction("Edit", "Albums", new { id = track.AlbumId });
            }

            return View(track);
        }


        public IActionResult DeleteTrack(Guid id)
        {
            var albumId = _trackService.GetDetailsForTrack(id).AlbumId;
            _trackService.DeleteTrack(id);
            return RedirectToAction("Edit", new { id = albumId });
        }

        [HttpGet]
        public IActionResult AddToPlaylist(Guid trackId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var playlists = _userPlaylistService.GetAllPlaylists(userId);
            ViewBag.TrackId = trackId;
            return View("AddToPlaylist", playlists);
        }

        //Action to handle adding the track to a playlist
        [HttpPost]
        public IActionResult AddTrackToPlaylist([Bind("TrackId,UserPlaylistId,Id")] TrackInPlaylist model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var track = _trackService.GetDetailsForTrack(model.TrackId);
            var result = _userPlaylistService.addTrackToPlaylist(userId, model);
            if (result)
            {
                return RedirectToAction("Details", new { id = track.AlbumId });
            }
            return BadRequest("Unable to add track to playlist."); // Handle error
        }


        private bool AlbumExists(Guid id)
        {
            return _albumService.GetAllAlbums().Any(e => e.Id == id);
        }
    }
}
