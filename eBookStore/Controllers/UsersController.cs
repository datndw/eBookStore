using eBookStore.Filters;
using eBookStore.Models;
using eBookStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eBookStore.Controllers
{
    [Authorize("Admin")]
    public class UsersController : Controller
    {
        private readonly string UserApiUrl = "https://localhost:9999/odata/users";
        private readonly string PublisherApiUrl = "https://localhost:9999/odata/publishers";
        private readonly string RoleApiUrl = "https://localhost:9999/odata/roles";
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

            var respone = await _client.GetAsync($"{UserApiUrl}");
            string strData = await respone.Content.ReadAsStringAsync();

            List<UserDTO> items = new();

            dynamic temp = JObject.Parse(strData);

            if ((JArray)temp.value == null)
            {
                return View("NotFound");
            }

            items = ((JArray)temp.value).Select(x => new UserDTO
            {
                Id = (int)x["Id"],
                EmailAddress = (string)x["EmailAddress"],
                Source = (string)x["Source"],
                FirstName = (string)x["FirstName"],
                MiddleName = (string)x["MiddleName"],
                LastName = (string)x["LastName"],
                RoleDesc = (string)x["RoleDesc"],
                PublisherName = (string)x["PublisherName"],
                HireDate = (DateTime?)x["HireDate"]
            }).ToList();
            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
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

        public async Task<IActionResult> Create()
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }

            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");
            var rolesResponse = await _client.GetAsync($"{RoleApiUrl}");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["Publisher"] = new SelectList(publishers, "Id", "Name");

            string rolesStrData = await rolesResponse.Content.ReadAsStringAsync();
            dynamic rolesTemp = JObject.Parse(rolesStrData);
            var roles = ((JArray)rolesTemp.value).Select(x => new RoleDTO
            {
                Id = (int)x["Id"],
                Desc = (string)x["Desc"]
            }).ToList();
            ViewData["Role"] = new SelectList(roles, "Id", "Desc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailAddress,Password,Source,FirstName,MiddleName,LastName,RoleId,PublisherId,HireDate")] UserDTO user)
        {
            if (!SetUpHttpClient())
            {
                return View("Unauthorized");
            }
            ModelState.Remove("RoleDesc");
            ModelState.Remove("PublisherName");
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(new
                {
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

                var response = await _client.PostAsync($"{UserApiUrl}", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = await response.Content.ReadAsStringAsync();
            }

            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");
            var rolesResponse = await _client.GetAsync($"{RoleApiUrl}");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["Publisher"] = new SelectList(publishers, "Id", "Name");

            string rolesStrData = await rolesResponse.Content.ReadAsStringAsync();
            dynamic rolesTemp = JObject.Parse(rolesStrData);
            var roles = ((JArray)rolesTemp.value).Select(x => new RoleDTO
            {
                Id = (int)x["Id"],
                Desc = (string)x["Desc"]
            }).ToList();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Desc");
            return View(user);
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


            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");
            var rolesResponse = await _client.GetAsync($"{RoleApiUrl}");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["Publisher"] = new SelectList(publishers, "Id", "Name");

            string rolesStrData = await rolesResponse.Content.ReadAsStringAsync();
            dynamic rolesTemp = JObject.Parse(rolesStrData);
            var roles = ((JArray)rolesTemp.value).Select(x => new RoleDTO
            {
                Id = (int)x["Id"],
                Desc = (string)x["Desc"]
            }).ToList();
            ViewData["Role"] = new SelectList(roles, "Id", "Desc");

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailAddress,Password,Source,FirstName,MiddleName,LastName,RoleId,RoleDesc,PublisherId,PublisherName,HireDate")] UserDTO user)
        {
            if (!SetUpHttpClient())
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
                string content = JsonSerializer.Serialize(new UserDTO
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

            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");
            var rolesResponse = await _client.GetAsync($"{RoleApiUrl}");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["Publisher"] = new SelectList(publishers, "Id", "Name");

            string rolesStrData = await rolesResponse.Content.ReadAsStringAsync();
            dynamic rolesTemp = JObject.Parse(rolesStrData);
            var roles = ((JArray)rolesTemp.value).Select(x => new RoleDTO
            {
                Id = (int)x["Id"],
                Desc = (string)x["Desc"]
            }).ToList();

            ViewData["Role"] = new SelectList(roles, "Id", "Desc");
            return View(user);
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

            if (HttpContext.Session.GetInt32("id").Value == id)
            {
                ViewBag.Error = "Cannot remove yourself";
            }

            var response = await _client.DeleteAsync($"{UserApiUrl}({id})");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
