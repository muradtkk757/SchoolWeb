using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Academy.MVC.Areas.StudentPanel.Controllers
{
    [Area("StudentPanel")]
    public class AttendanceController : Controller
    {
        private readonly ApiClient _apiClient;

        public AttendanceController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            // Cookie-d?n t?l?b? m?lumatlar?n? ç?xarmaq
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Home", new { area = "" });

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            if (string.IsNullOrEmpty(userName)) return RedirectToAction("Index", "Home", new { area = "" });

            // Bütün davamiyy?t jurnal?ndan ad? bu username-? uy?un g?l?n s?tirl?ri filter edirik
            // API-d? birba?a username/ID il? g?tirm?k daha düzgün olar "api/attendances/mystudents". 
            // Müv?qq?ti all attendances-d?n tap?r?q
            
            var allAttendances = await _apiClient.GetAsync<List<AttendanceDto>>("api/attendances") ?? new List<AttendanceDto>();
            
            // Formatlama c?hdl?ri: userName ad?t?n student-p301-1 olur, Name is? Student-P301-1
            var myHistory = allAttendances
                .Where(a => !string.IsNullOrEmpty(a.StudentName) && 
                            a.StudentName.Replace("-", "").ToLower() == userName.ToLower() ||
                            a.StudentName?.ToLower() == userName.ToLower())
                .OrderByDescending(a => a.Date)
                .ToList();

            ViewBag.StudentName = myHistory.FirstOrDefault()?.StudentName ?? userName;
            ViewBag.PresenceCount = myHistory.Count(a => a.AttendanceStatus == "Present");
            ViewBag.AbsenceCount = myHistory.Count(a => a.AttendanceStatus == "Absent");
            ViewBag.LateCount = myHistory.Count(a => a.AttendanceStatus == "Late");

            return View(myHistory);
        }
    }
}