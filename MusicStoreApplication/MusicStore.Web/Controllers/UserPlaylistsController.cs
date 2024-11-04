using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Domain.Domain;
using MusicStore.Repository;
using MusicStore.Service.Implementation;
using MusicStore.Service.Interface;


namespace MusicStore.Web.Controllers
{
    public class UserPlaylistsController : Controller
    {
        private readonly IUserPlaylistService _userPlaylistService;
        private readonly ITrackService _trackService;

        public UserPlaylistsController(IUserPlaylistService userPlaylistService, ITrackService trackService)
        {
            _userPlaylistService = userPlaylistService;
            _trackService = trackService;
        }

        // GET: UserPlaylists
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {

                ViewBag.LoggedIn = false;
            }
            else
            {
                ViewBag.LoggedIn = true;
            }

            return View(_userPlaylistService.GetAllPlaylists(userId));
        }

        // GET: UserPlaylists/Details/5
        public IActionResult Details(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = _userPlaylistService.GetDetailsForPlaylist(userId, id);
            if (userPlaylist == null)
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

            return View(userPlaylist);
        }

        // GET: UserPlaylists/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.CurrentUserId = userId;
            return View();
        }

        // POST: UserPlaylists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PlaylistName,Image,UserId,Id")] UserPlaylist userPlaylist)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                userPlaylist.Id = Guid.NewGuid();
                _userPlaylistService.CreateNewPlaylist(userId, userPlaylist);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CurrentUserId = userId;
            return View(userPlaylist);
        }

        // GET: UserPlaylists/Edit/5
        public IActionResult Edit(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = _userPlaylistService.GetDetailsForPlaylist(userId, id);
            if (userPlaylist == null)
            {
                return NotFound();
            }
            ViewBag.CurrentUserId = userId;
            return View(userPlaylist);
        }

        // POST: UserPlaylists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("PlaylistName,Image,UserId,Id")] UserPlaylist userPlaylist)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != userPlaylist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userPlaylistService.UpdateExistingPlaylist(userPlaylist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Details", new { id = userPlaylist.Id });
            }
            ViewBag.CurrentUserId = userId;
            return View(userPlaylist);
        }

        // GET: UserPlaylists/Delete/5
        public IActionResult Delete(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = _userPlaylistService.GetDetailsForPlaylist(userId, id);
            if (userPlaylist == null)
            {
                return NotFound();
            }

            return View(userPlaylist);
        }

        // POST: UserPlaylists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _userPlaylistService.DeletePlaylist(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoveTrackFromPlaylist(Guid TrackInPlaylistId, Guid PlaylistId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userPlaylistService.RemoveTrackFromPlaylist(userId, TrackInPlaylistId, PlaylistId);

            return RedirectToAction("Details", new { id = PlaylistId });
        }


        private bool UserPlaylistExists(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _userPlaylistService.GetAllPlaylists(userId).Any(e => e.Id == id);
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string fileName = "MyPlaylists.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("MyPlaylists");
                worksheet.Cell(1, 1).Value = "Playlist name";
                worksheet.Cell(1, 2).Value = "Total duration";

                var data = _userPlaylistService.GetAllPlaylistsForExport().FindAll(z => z.User.Id == userId);

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data[i];
                    var totalDuration =  _trackService.CalculateTotalDuration(item.Id);
                    string formattedDuration;
                    if (totalDuration[0] > 0)
                    {
                        formattedDuration = $"{totalDuration[0]} hr {totalDuration[1]} min";
                    }
                    else
                    {
                        formattedDuration = $"{totalDuration[1]} min {totalDuration[2]} sec";
                    }

                    worksheet.Cell(i + 2, 1).Value = item.PlaylistName;
                    worksheet.Cell(i + 2, 2).Value = formattedDuration;
                    var total = 0;
                    for (int j = 0; j < item.TracksInPlaylist.Count(); j++)
                    {
                        worksheet.Cell(1, 3 + j).Value = "Song - " + (j + 1);
                        worksheet.Cell(i + 2, 3 + j).Value = item.TracksInPlaylist.ElementAt(j).Track.TrackName;
                    }
                   

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        }
}
