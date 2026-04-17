using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Academy.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _apiClient;

        public AccountController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var loginData = new { UserName = username, Password = password };
                var response = await _apiClient.PostAsync<TokenResponse>("api/auth/login", loginData);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Tokeni cookieParser v? ya session-da saxlamaq olar
                    Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    // Token-i decode edib rolu tapmaq
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(response.Token);
                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

                    if (roleClaim != null)
                    {
                        if (roleClaim.Value == "Teacher")
                            return RedirectToAction("Index", "Group", new { area = "TeacherPanel" });
                        else if (roleClaim.Value == "Student")
                            return RedirectToAction("Index", "Attendance", new { area = "StudentPanel" });
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                
                TempData["ErrorMessage"] = "Invalid username or password";
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                TempData["ErrorMessage"] = "API connection failed or invalid credentials";
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Home");
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}