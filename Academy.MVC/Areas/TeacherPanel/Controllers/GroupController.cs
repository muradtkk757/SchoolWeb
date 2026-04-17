using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Academy.MVC.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    public class GroupController : Controller
    {
        private readonly ApiClient _apiClient;

        public GroupController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Cookie-dən tokeni götürürük
                var token = Request.Cookies["AuthToken"];
                
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                // Tokendən istifadəçi İD-sini (AppUserId) çıxarırıq
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                
                var groups = await _apiClient.GetAsync<List<GroupDto>>("api/groups");

                if (groups is null)
                {
                    ViewBag.Error = "Could not load groups from API.";
                    return View(Enumerable.Empty<GroupDto>());
                }

                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    
                    // Müəllimlərin siyahısını cəkirik ki, AppUserId-yə əsasən Teacher obyektini tapaq
                    var teachers = await _apiClient.GetAsync<List<TeacherDto>>("api/teachers");
                    var currentTeacher = teachers?.FirstOrDefault(t => t.AppUserId == userId);

                    if (currentTeacher != null && currentTeacher.Name != null)
                    {
                        // Qrupları tapılan müəllimin "Name" (Ad-Soyad) məlumatına görə filter edirik
                        groups = groups.Where(g => g.TeacherName != null && g.TeacherName.Equals(currentTeacher.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else
                    {
                        // Əgər istifadəçiyə uyğun teacher tapılmadısa, boş siyahı göstərsin
                        groups = new List<GroupDto>();
                    }
                }
                else
                {
                    groups = new List<GroupDto>();
                }

                return View(groups);
            }
            catch
            {
                ViewBag.Error = "Academy API is not reachable.";
                return View(Enumerable.Empty<GroupDto>());
            }
        }
    }
}
