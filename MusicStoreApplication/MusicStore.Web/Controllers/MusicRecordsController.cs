using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using MusicStore.Domain.Domain;

namespace MusicStore.Web.Controllers
{
    public class MusicRecordsController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5147/api/admin/products";
            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<MusicRecord>>().Result;
            return View(data);
        }
    }
}
