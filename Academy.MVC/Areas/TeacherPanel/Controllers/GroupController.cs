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

                // Tokendən istifadəçi adını çıxarırıq
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                var usernameClaimName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                
                var groups = await _apiClient.GetAsync<List<GroupDto>>("api/groups");

                if (groups is null)
                {
                    ViewBag.Error = "Could not load groups from API.";
                    return View(Enumerable.Empty<GroupDto>());
                }

                // Yalnız hesaba daxil olan müəllimin qruplarını filter edirik.
                // Token-də userName (NameClaim) gəlir. Teacher adı ilə eyni olduğuna fərz edərək:
                if (usernameClaimName != null)
                {
                    var username = usernameClaimName.Value;
                    groups = groups.Where(g => g.TeacherName != null && g.TeacherName.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
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
