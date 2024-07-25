using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Users.Api.Models;

namespace UsersMvc.Controllers
{
    public class UsersMvcController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersMvcController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<User>>("https://localhost:44390/api/Users");
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44390/api/Users", user);
            if (response.IsSuccessStatusCode)
            {               
                TempData["SuccessMessage"] = "Novo usuário criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
                       
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _httpClient.GetFromJsonAsync<User>($"https://localhost:44390/api/Users/{id}");
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:44390/api/Users/{id}", user);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Usuário editado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _httpClient.GetFromJsonAsync<User>($"https://localhost:44390/api/Users/{id}");
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UsersMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44390/api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Usuário deletado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View("Error"); 
        }
    }
}
