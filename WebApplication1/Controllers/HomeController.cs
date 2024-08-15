using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using rest_api.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController(IHttpClientFactory clientFactory) : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var httpClient = clientFactory.CreateClient();
            var res = await httpClient.GetAsync("https://dummyjson.com/todos");

            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                TodosModel? todos = JsonSerializer.Deserialize<TodosModel>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (todos == null) { return RedirectToAction("index"); }
                return View(todos.todos);
            }
            return RedirectToAction("index");
        }

        public IActionResult Create()
        {
            TodoModel todo = new();
            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoModel todoModel)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(todoModel), Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync("https://dummyjson.com/todos/add", httpContent);
            
            if (res.IsSuccessStatusCode)
            {
                return View("success");
            }
            
            return View("failed");
            
        }
        public IActionResult Update(int id)
        {
            TodoModel todo = new TodoModel();
            todo.id = id;
            return View(todo);
        }
        [HttpPut]
        public async Task<IActionResult> Update(TodoModel todoModel)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(todoModel), Encoding.UTF8, "application/json");
            var res = await httpClient.PutAsync($"https://jsonplaceholder.typicode.com/posts/{todoModel.id}", httpContent);
            if (res.IsSuccessStatusCode)
            {
                return View("success");
            }

            return View("failed");

        }






    }
}
