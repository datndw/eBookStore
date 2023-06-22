using eBookStore.Filters;
using eBookStore.Models;
using eBookStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace eBookStore.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _client;
        private readonly string AuthenticationApiUrl = "https://localhost:9999/api/Authentication";
        private readonly string UserApiUrl = "https://localhost:9999/odata/users";
        public AuthenticationController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("jwtToken")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("EmailAddress,Password,Source,FirstName,MiddleName,LastName")] UserDTO user)
        {
            user.RoleId = 3;
            user.PublisherId = 1;
            ModelState.Remove("RoleDesc");
            ModelState.Remove("PublisherName");
            if (ModelState.IsValid)
            {
                
                string content = System.Text.Json.JsonSerializer.Serialize(new
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
                    HireDate = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'")
                });
                var data = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{UserApiUrl}", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = await response.Content.ReadAsStringAsync();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateRequest authenticateRequest)
        {
            if (ModelState.IsValid)
            {
                string content = System.Text.Json.JsonSerializer.Serialize(authenticateRequest);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{AuthenticationApiUrl}", data);
                string strData = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = strData;
                }
                else
                {
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(strData);
                    if (authenticationResponse != null)
                    {
                        HttpContext.Session.SetString("jwtToken", authenticationResponse.Token);
                        HttpContext.Session.SetString("email", authenticationResponse.EmailAddress);
                        HttpContext.Session.SetInt32("id", authenticationResponse.Id);
                        HttpContext.Session.SetString("role", authenticationResponse.Role);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("Login", authenticateRequest);

        }

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
