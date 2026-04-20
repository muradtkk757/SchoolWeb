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

        public async Task<IActionResult> Take(int groupId)
        {
            var group = await _apiClient.GetAsync<GroupDto>($"api/groups/{groupId}");
            if (group == null) return NotFound();

            ViewBag.GroupId = groupId;
            ViewBag.GroupName = group.Name;
            
            return View(group.Students);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(IFormCollection form)
        {
            var attendanceList = new List<CreateAttendanceDto>();
            
            if (form == null || form.Keys.Count == 0)
            {
                TempData["ErrorMessage"] = "Form boş göndərildi!";
                return RedirectToAction(nameof(Index));
            }

            int.TryParse(form["groupId"].ToString(), out int groupId);
            DateTime.TryParse(form["date"].ToString(), out DateTime date);
            
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("statuses["))
                {
                    var idString = key.Replace("statuses[", "").Replace("]", "");
                    if (int.TryParse(idString, out int studentId))
                    {
                        var statusValue = form[key].ToString();
                        if (Enum.TryParse(statusValue, true, out AttendanceStatusDto parsedStatus))
                        {
                            attendanceList.Add(new CreateAttendanceDto
                            {
                                StudentId = studentId,
                                Date = date == default ? DateTime.Now : date,
                                AttendanceStatus = parsedStatus
                            });
                        }
                    }
                }
            }

            if(attendanceList.Any())
            {
                var response = await _apiClient.PostAsync<object>("api/attendances/bulk", attendanceList);
                if (response == null) 
                {
                    // ApiClient post xəta olduqca default(null) qaytarır
                    TempData["ErrorMessage"] = "Məlumat API-yə göndərilərkən xəta baş verdi (Server xətası). Console Output-a baxın.";
                }
                else 
                {
                    TempData["SuccessMessage"] = "Məlumatlar uğurla qeyd olundu!";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Heç bir davamiyyət (status) tapılmadı.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewPast(int groupId)
        {
            // Bütünlükdə API-dan məlumatı çekirik
            var allAttendances = await _apiClient.GetAsync<List<AttendanceDto>>("api/attendances");
            var group = await _apiClient.GetAsync<GroupDto>($"api/groups/{groupId}");

            if (allAttendances == null)
            {
                TempData["ErrorMessage"] = "API-dən məlumatları (GET api/attendances) çəkmək mümkün olmadı. Backend 500 error verir.";
                return RedirectToAction(nameof(Index));
            }

            if (allAttendances.Count == 0)
            {
                ViewBag.Error = "API uğurla çalışır, lakin verilənlər bazasında (DB) HEÇ BİR DAVAMİYYƏT YOXDUR! Demək ki, Submit edəndə Database-ə yazılmır.";
                return View(new List<AttendanceDto>());
            }

            // Enum olaraq 0, 1 rəqəmi gəlibsə Text-ə çevirək ki View qəşəng oxusun
            foreach (var a in allAttendances)
            {
                if (a.AttendanceStatus == "0" || string.Equals(a.AttendanceStatus, "Present", StringComparison.OrdinalIgnoreCase)) a.AttendanceStatus = "Present";
                else if (a.AttendanceStatus == "1" || string.Equals(a.AttendanceStatus, "Absent", StringComparison.OrdinalIgnoreCase)) a.AttendanceStatus = "Absent";
                else if (a.AttendanceStatus == "2" || string.Equals(a.AttendanceStatus, "Late", StringComparison.OrdinalIgnoreCase)) a.AttendanceStatus = "Late";
                else if (a.AttendanceStatus == "3" || string.Equals(a.AttendanceStatus, "Excused", StringComparison.OrdinalIgnoreCase)) a.AttendanceStatus = "Excused";
            }

            // Qrupa görə filter edirik
            var groupStudentNames = group?.Students?.Select(s => s.Name?.Trim().ToLower()).ToList() ?? new List<string>();

            // GroupName birbaşa qayıdırsa onu test edirik:
            var filteredAttendances = allAttendances.Where(a => 
                (!string.IsNullOrEmpty(a.GroupName) && string.Equals(a.GroupName, group?.Name, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(a.StudentName) && groupStudentNames.Contains(a.StudentName.Trim().ToLower()))
            ).OrderByDescending(a => a.Date).ToList();

            ViewBag.GroupName = group?.Name ?? "Unknown";
            ViewBag.GroupId = groupId;

            // Əgər filter heç nə tapmadısa deməli mapping GroupName-i null qoyur. 
            // Bizə gələn adları yoxlayaq
            if (!filteredAttendances.Any() && allAttendances.Any())
            {
                // Səhvi silib BÜTÜN DÜNYANI GÖSTƏRMƏK ƏVƏZİNƏ yalnız o uşaqları ID ilə match etməyə çalışaq 
                // amma ID Dto-da gəlmir API-dan...
                ViewBag.Error = $"Sizin qrup ({group?.Name}) üçün heçnə tapılmadı. Görüntü xətasını test etmək üçün SİSTEMDƏ OLAN BÜTÜN qeydləri görürsünüz:";
                return View(allAttendances.OrderByDescending(a => a.Date).ToList());
            }

            return View(filteredAttendances);
        }
    }
}