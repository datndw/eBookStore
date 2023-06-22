using eBookStore.Filters;
using eBookStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eBookStore.Controllers
{
    [Authorize("Admin")]
    public class AuthorsController : Controller
    {
        private readonly string AuthorApiUrl = "https://localhost:9999/odata/authors";
        private HttpClient _client;

        private bool SetUpHttpClient()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            string token = HttpContext.Session.GetString("jwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        public async Task<IActionResult> Index()
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            List<AuthorDTO> items = new();

            var respone = await _client.GetAsync($"{AuthorApiUrl}");
            string strData = await respone.Content.ReadAsStringAsync();

            dynamic temp = JObject.Parse(strData);
            if ((JArray)temp.value != null)
            {
                items = ((JArray)temp.value).Select(x => new AuthorDTO
                {
                    Id = (int)x["Id"],
                    LastName = (string)x["LastName"],
                    FirstName = (string)x["FirstName"],
                    Phone = (string)x["Phone"],
                    Address = (string)x["Address"],
                    City = (string)x["City"],
                    State = (string)x["State"],
                    Zip = (int)x["Zip"],
                    EmailAddress = (string)x["EmailAddress"]
                }).ToList();
            }

            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            var response = await _client.GetAsync($"{AuthorApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var author = new AuthorDTO
            {
                Id = (int)temp["Id"],
                LastName = (string)temp["LastName"],
                FirstName = (string)temp["FirstName"],
                Phone = (string)temp["Phone"],
                Address = (string)temp["Address"],
                City = (string)temp["City"],
                State = (string)temp["State"],
                Zip = (int)temp["Zip"],
                EmailAddress = (string)temp["EmailAddress"]
            };

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,Phone,Address,City,State,Zip,EmailAddress")] AuthorDTO author)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            string content = JsonSerializer.Serialize(new
            {
                Id = author.Id,
                LastName = author.LastName,
                FirstName = author.FirstName,
                Phone = author.Phone,
                Address = author.Address,
                City = author.City,
                State = author.State,
                Zip = author.Zip,
                EmailAddress = author.EmailAddress
            });
            var data = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{AuthorApiUrl}", data);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = await response.Content.ReadAsStringAsync();

            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"{AuthorApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var author = new AuthorDTO
            {
                Id = (int)temp["Id"],
                LastName = (string)temp["LastName"],
                FirstName = (string)temp["FirstName"],
                Phone = (string)temp["Phone"],
                Address = (string)temp["Address"],
                City = (string)temp["City"],
                State = (string)temp["State"],
                Zip = (int)temp["Zip"],
                EmailAddress = (string)temp["EmailAddress"]
            };

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,Phone,Address,City,State,Zip,EmailAddress")] AuthorDTO author)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            if (id != author.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            string content = JsonSerializer.Serialize(new
            {
                Id = author.Id,
                LastName = author.LastName,
                FirstName = author.FirstName,
                Phone = author.Phone,
                Address = author.Address,
                City = author.City,
                State = author.State,
                Zip = author.Zip,
                EmailAddress = author.EmailAddress
            });
            var data = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{AuthorApiUrl}({id})", data);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = await response.Content.ReadAsStringAsync();

            return View(author);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"{AuthorApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var author = new AuthorDTO
            {
                Id = (int)temp["Id"],
                LastName = (string)temp["LastName"],
                FirstName = (string)temp["FirstName"],
                Phone = (string)temp["Phone"],
                Address = (string)temp["Address"],
                City = (string)temp["City"],
                Zip = (int)temp["Zip"],
                EmailAddress = (string)temp["EmailAddress"]
            };

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.DeleteAsync($"{AuthorApiUrl}({id})");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
