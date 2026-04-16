using Academy.BLL.DTOs;
using Academy.MVC.Services;
using Microsoft.AspNetCore.Mvc;

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
                var groups = await _apiClient.GetAsync<List<GroupDto>>("api/groups");

                if (groups is null)
                {
                    ViewBag.Error = "Could not load groups from API.";
                    return View(Enumerable.Empty<GroupDto>());
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
