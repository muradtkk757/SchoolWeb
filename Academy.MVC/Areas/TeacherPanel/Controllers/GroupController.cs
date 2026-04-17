using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                var userNameClaim = jwtToken.Claims.FirstOrDefault(c => 
                    c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                var userName = userNameClaim?.Value;
                
                var allGroups = await _apiClient.GetAsync<List<GroupDto>>("api/groups") ?? new List<GroupDto>();
                
                if (string.IsNullOrEmpty(userName))
                {
                    ViewBag.Error = "Token içindən istifadəçi adı (username) tapılmadı.";
                    return View(allGroups);
                }
                
                var teachers = await _apiClient.GetAsync<List<TeacherDto>>("api/teachers");
                
                TeacherDto? currentTeacher = null;
                if (teachers != null)
                {
                    currentTeacher = teachers.FirstOrDefault(t => 
                        (t.Name != null && t.Name.Replace(" ", "").ToLower() == userName.ToLower()) ||
                        userName.ToLower().Contains(t.Name?.Replace(" ", "").ToLower() ?? "")
                    );
                }

                if (currentTeacher != null)
                {
                    // Artıq API-dən gələn siyahıda TeacherName var, sadəcə filter edirik:
                    var myGroups = allGroups.Where(g => 
                        !string.IsNullOrEmpty(g.TeacherName) && 
                        g.TeacherName.Equals(currentTeacher.Name, StringComparison.OrdinalIgnoreCase)
                    ).ToList();

                    // Əgər həqiqətən bu müəllimə qrup təyin olunmayıbsa
                    if (!myGroups.Any())
                    {
                        ViewBag.Error = null; // Əvvəlki "Teacher null" xətanı sildik, artıq sistem işləyir, sadəcə qrup yoxdur.
                    }

                    return View(myGroups);
                }

                ViewBag.Error = $"Sizin istifadəçi adınıza ('{userName}') uyğun Müəllim tapılmadı.";
                return View(allGroups); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Xəta: {ex.Message}";
                return View(new List<GroupDto>());
            }
        }
    }
}
