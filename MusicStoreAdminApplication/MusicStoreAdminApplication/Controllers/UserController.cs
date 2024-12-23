﻿using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using ExcelDataReader;
using MusicStoreAdminApplication.Models;

namespace MusicStoreAdminApplication.Controllers
{
    public class UserController : Controller
    {
        //public UserManager<EShopApplicationUser> usermanager;
        //public UserController(UserManager<EShopApplicationUser> usermanager)
        //{
        //    this.usermanager = usermanager;
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportUsers(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<User> users = getAllUsersFromFile(file.FileName);
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5059/api/Admin/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "User");

        }

        private List<User> getAllUsersFromFile(string fileName)
        {
            List<User> users = new List<User>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new Models.User
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString()
                        });
                    }

                }
            }
            return users;

        }
    }
}
