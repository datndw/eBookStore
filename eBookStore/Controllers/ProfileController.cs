using eBookStore.Filters;
using eBookStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eBookStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly string UserApiUrl = "https://localhost:9999/odata/users";
        private HttpClient _client;

        private bool? SetUpHttpClient()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            string token = HttpContext.Session.GetString("jwtToken");
            int? id = HttpContext.Session.GetInt32("id");

            if (string.IsNullOrEmpty(token) || id == null)
            {
                return false;
            }

            if (id.Value == 0)
            {
                return null;
            }

            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        public async Task<IActionResult> Index()
        {
            if (SetUpHttpClient() == null)
            {
                return NotFound();
            }

            if (!SetUpHttpClient().Value)
            {
                return View("Unauthorized");
            }

            int? id = HttpContext.Session.GetInt32("id");

            var response = await _client.GetAsync($"{UserApiUrl}({id.Value})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var user = new UserDTO
            {
                Id = (int)temp["Id"],
                EmailAddress = (string)temp["EmailAddress"],
                Source = (string)temp["Source"],
                FirstName = (string)temp["FirstName"],
                MiddleName = (string)temp["MiddleName"],
                LastName = (string)temp["LastName"],
                RoleDesc = (string)temp["RoleDesc"],
                PublisherName = (string)temp["PublisherName"],
                HireDate = (DateTime?)temp["HireDate"]
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (SetUpHttpClient() == null)
            {
                return NotFound();
            }

            if (!SetUpHttpClient().Value)
            {
                return View("Unauthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"{UserApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var user = new UserDTO
            {
                Id = (int)temp["Id"],
                EmailAddress = (string)temp["EmailAddress"],
                Password = (string)temp["Password"],
                Source = (string)temp["Source"],
                FirstName = (string)temp["FirstName"],
                MiddleName = (string)temp["MiddleName"],
                LastName = (string)temp["LastName"],
                RoleId = (int)temp["RoleId"],
                RoleDesc = (string)temp["RoleDesc"],
                PublisherId = (int)temp["PublisherId"],
                PublisherName = (string)temp["PublisherName"],
                HireDate = (DateTime?)temp["HireDate"]
            };
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailAddress,Password,Source,FirstName,MiddleName,LastName,RoleId,RoleDesc,PublisherId,PublisherDesc,HireDate")] UserDTO user)
        {
            if (SetUpHttpClient() == null)
            {
                return NotFound();
            }
            if (!SetUpHttpClient().Value)
            {
                return View("Unauthorized");
            }

            if (id != user.Id)
            {
                return NotFound();
            }
            ModelState.Remove("RoleDesc");
            ModelState.Remove("PublisherName");
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(new
                {
                    Id = user.Id,
                    EmailAddress = user.EmailAddress,
                    Password = user.Password,
                    Source = user.Source,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    RoleId = user.RoleId,
                    RoleDesc = "",
                    PublisherId = user.PublisherId,
                    PublisherName = "",
                    HireDate = DateTime.Parse(DateTime.Parse(user.HireDate.ToString()).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"))
                });
                var data = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{UserApiUrl}({id})", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = await response.Content.ReadAsStringAsync();
            }

            return View(user);
        }
    }
}
