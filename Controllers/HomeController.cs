using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoConsumeAPI.Models;


namespace TodoConsumeAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        string Baseurl = "https://localhost:44393/";
        public async Task<IActionResult> IndexAsync()
        {
            // Do this to avoid Untrusted root
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            List<TodoItem> TodoItems = new List<TodoItem>();
            // Pass the handler to httpclient to avoid untrusted root
            using (var client = new HttpClient(clientHandler))
            {
                // Pass service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                // Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Send request to web api REST service method GetTodoItems using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/todoitems");
                // Check result
                if (Res.IsSuccessStatusCode)
                {
                    //Store the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserialize response recieved from web api and store into TodoItems list
                    TodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(Response);
                }
                // Pass list to view
                return View(TodoItems);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
