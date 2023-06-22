using eBookStore.Filters;
using eBookStore.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
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
    public class BooksController : Controller
    {
        private HttpClient _client;
        private readonly string BookApiUrl = "https://localhost:9999/odata/books";
        private readonly string PublisherApiUrl = "https://localhost:9999/odata/publishers";

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

        public async Task<IActionResult> Index(string searchValue)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }

            HttpResponseMessage response = new();
            List<BookDTO> items = new();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                response = double.TryParse(searchValue, out double value)
                    ? await _client.GetAsync($"{BookApiUrl}?$filter=Price eq {value} " +
                    $"or Advance eq {value} " +
                    $"or Royalty eq {value} " +
                    $"or YtdSales eq {value}")
                    : await _client.GetAsync($"{BookApiUrl}?$filter=contains(tolower(Title), tolower('{searchValue}')) " +
                    $"or contains(tolower(Type), tolower('{searchValue}')) " +
                    $"or contains(tolower(Notes), tolower('{searchValue}'))");
            }
            else
            {
                response = await _client.GetAsync($"{BookApiUrl}");
            }

            string strData = await response.Content.ReadAsStringAsync();

            dynamic temp = JObject.Parse(strData);
            if (((JArray)temp.value) != null)
            {
                items = ((JArray)temp.value).Select(x => new BookDTO
                {
                    Id = (int)x["Id"],
                    Title = (string)x["Title"],
                    Type = (string)x["Type"],
                    PublisherId = (int)x["PublisherId"],
                    Price = (double)x["Price"],
                    Advance = (double)x["Advance"],
                    Royalty = (double)x["Royalty"],
                    YtdSales = (double)x["YtdSales"],
                    Notes = (string)x["Notes"],
                    PublishedDate = DateTime.Parse(x["PublishedDate"].ToString()).Date
                }).ToList();
            }
            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            var response = await _client.GetAsync($"{BookApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }
            var book = new BookDTO
            {
                Id = (int)temp["Id"],
                Title = (string)temp["Title"],
                Type = (string)temp["Type"],
                PublisherId = (int)temp["PublisherId"],
                Price = (double)temp["Price"],
                Advance = (double)temp["Advance"],
                Royalty = (double)temp["Royalty"],
                YtdSales = (double)temp["YtdSales"],
                Notes = (string)temp["Notes"],
                PublishedDate = DateTime.Parse(temp["PublishedDate"].ToString()).Date
            };

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }

            List<PublisherDTO> publishers = new();
            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            if ((JArray)publishersTemp.value != null)
            {
                publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
                {
                    Id = (int)x["Id"],
                    Name = (string)x["Name"]
                }).ToList();
            }

            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Type,PublisherId,Price,Advance,Royalty,YtdSales,Notes,PublishedDate")] BookDTO book)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(new
                {
                    Title = book.Title,
                    Type = book.Type,
                    PublisherId = book.PublisherId,
                    Price = book.Price,
                    Advance = book.Advance,
                    Royalty = book.Royalty,
                    YtdSales = book.YtdSales,
                    Notes = book.Notes,
                    PublishedDate = book.PublishedDate.Value.ToString("yyyy-MM-dd")
                });
                var data = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BookApiUrl}", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = await response.Content.ReadAsStringAsync();

                List<PublisherDTO> publishers = new();
                var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");

                string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
                dynamic publishersTemp = JObject.Parse(publishersStrData);
                if ((JArray)publishersTemp.value != null)
                {
                    publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
                    {
                        Id = (int)x["Id"],
                        Name = (string)x["Name"]
                    }).ToList();
                }

                ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");
            }

            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"{BookApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var book = new BookDTO
            {
                Id = (int)temp["Id"],
                Title = (string)temp["Title"],
                Type = (string)temp["Type"],
                PublisherId = (int)temp["PublisherId"],
                Price = (double)temp["Price"],
                Advance = (double)temp["Advance"],
                Royalty = (double)temp["Royalty"],
                YtdSales = (double)temp["YtdSales"],
                Notes = (string)temp["Notes"],
                PublishedDate = DateTime.Parse(temp["PublishedDate"].ToString())
            };

            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,PublisherId,Price,Advance,Royalty,YtdSales,Notes,PublishedDate")] BookDTO book)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(new
                {
                    Id = book.Id,
                    Title = book.Title,
                    Type = book.Type,
                    PublisherId = book.PublisherId,
                    Price = book.Price,
                    Advance = book.Advance,
                    Royalty = book.Royalty,
                    YtdSales = book.YtdSales,
                    Notes = book.Notes,
                    PublishedDate = book.PublishedDate.Value.ToString("yyyy-MM-dd")
                });
                var data = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{BookApiUrl}({id})", data);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = await response.Content.ReadAsStringAsync();
            }

            var publishersResponse = await _client.GetAsync($"{PublisherApiUrl}?$Select=Id,Name");

            string publishersStrData = await publishersResponse.Content.ReadAsStringAsync();
            dynamic publishersTemp = JObject.Parse(publishersStrData);
            var publishers = ((JArray)publishersTemp.value).Select(x => new PublisherDTO
            {
                Id = (int)x["Id"],
                Name = (string)x["Name"]
            }).ToList();
            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");

            return View(book);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"{BookApiUrl}({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("NotFound");
            }

            var book = new BookDTO
            {
                Id = (int)temp["Id"],
                Title = (string)temp["Title"],
                Type = (string)temp["Type"],
                PublisherId = (int)temp["PublisherId"],
                Price = (double)temp["Price"],
                Advance = (double)temp["Advance"],
                Royalty = (double)temp["Royalty"],
                YtdSales = (double)temp["YtdSales"],
                Notes = (string)temp["Notes"],
                PublishedDate = DateTime.Parse(temp["PublishedDate"].ToString()).Date
            };

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (!SetUpHttpClient())
            {
                return RedirectToAction("Index", "Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.DeleteAsync($"{BookApiUrl}({id})");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
