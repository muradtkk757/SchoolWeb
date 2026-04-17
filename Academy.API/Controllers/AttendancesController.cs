using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Core.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace Academy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("bypage")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            var attendances = await _attendanceService.GetListAsync(pageRequest);
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return Ok(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceDto createAttendanceDto)
        {
            await _attendanceService.AddAsync(createAttendanceDto);
            return Ok();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] List<CreateAttendanceDto> createAttendanceDtos)
        {
            foreach (var dto in createAttendanceDtos)
            {
                await _attendanceService.AddAsync(dto);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto updateAttendanceDto)
        {
            await _attendanceService.UpdateAsync(updateAttendanceDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.DeleteAsync(id);
            return Ok();
        }
    }
}