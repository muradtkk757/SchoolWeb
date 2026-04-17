using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Academy.MVC.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    public class AttendanceController : Controller
    {
        private readonly ApiClient _apiClient;

        public AttendanceController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Home", new { area = "" });

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var allGroups = await _apiClient.GetAsync<List<GroupDto>>("api/groups") ?? new List<GroupDto>();
            var teachers = await _apiClient.GetAsync<List<TeacherDto>>("api/teachers");

            TeacherDto? currentTeacher = null;
            if (teachers != null && userName != null)
            {
                currentTeacher = teachers.FirstOrDefault(t => 
                    (t.Name != null && t.Name.Replace(" ", "").ToLower() == userName.ToLower()) ||
                    userName.ToLower().Contains(t.Name?.Replace(" ", "").ToLower() ?? "")
                );
            }

            var myGroups = new List<GroupDto>();
            if (currentTeacher != null)
            {
                myGroups = allGroups.Where(g => 
                    !string.IsNullOrEmpty(g.TeacherName) && 
                    g.TeacherName.Equals(currentTeacher.Name, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View(myGroups);
        }

        [HttpGet]
        public async Task<IActionResult> Take(int groupId)
        {
            var group = await _apiClient.GetAsync<GroupDto>($"api/groups/{groupId}");
            if (group == null) return NotFound();

            ViewBag.GroupId = groupId;
            ViewBag.GroupName = group.Name;
            
            return View(group.Students);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int groupId, DateTime date, Dictionary<int, string> statuses)
        {
            var attendanceList = new List<CreateAttendanceDto>();
            
            foreach(var status in statuses)
            {
                // Enum Parsing
                if (Enum.TryParse(status.Value, out AttendanceStatusDto parsedStatus))
                {
                    attendanceList.Add(new CreateAttendanceDto
                    {
                        StudentId = status.Key,
                        Date = date,
                        AttendanceStatus = parsedStatus
                    });
                }
            }

            if(attendanceList.Any())
            {
                var response = await _apiClient.PostAsync<object>("api/attendances/bulk", attendanceList);
                TempData["SuccessMessage"] = "Attendance successfully recorded for the selected date!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewPast(int groupId)
        {
            var allAttendances = await _apiClient.GetAsync<List<AttendanceDto>>("api/attendances") ?? new List<AttendanceDto>();
            var group = await _apiClient.GetAsync<GroupDto>($"api/groups/{groupId}");
            
            // Sad?c? seçilmi? qrubun t?l?b?l?rin? aid attendance-l?ri filter ed?k
            var groupStudentNames = group?.Students.Select(s => s.Name).ToList() ?? new List<string?>();
            
            var groupAttendances = allAttendances
                .Where(a => a.StudentName != null && groupStudentNames.Contains(a.StudentName))
                .OrderByDescending(a => a.Date)
                .ToList();

            ViewBag.GroupName = group?.Name ?? "Unknown";
            ViewBag.GroupId = groupId;

            return View(groupAttendances);
        }
    }
}