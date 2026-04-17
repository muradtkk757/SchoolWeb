using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Core.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Academy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllWithDetailsAsync();

            return Ok(groups);
        }

        [HttpGet("bypage")]
        public async Task<IActionResult> GetGroups([FromQuery]PageRequest pageRequest)
        {
            var groups = await _groupService.GetListAsync(pageRequest);

            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            await _groupService.AddAsync(createGroupDto);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
        {
            await _groupService.UpdateAsync(updateGroupDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            await _groupService.DeleteAsync(id);
            return Ok();
        }
    }
}