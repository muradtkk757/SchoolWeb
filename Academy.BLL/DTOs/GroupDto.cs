using Academy.DAL.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class GroupStudentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class GroupDto : Dto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TeacherName { get; set; }
        public IEnumerable<GroupStudentDto> Students { get; set; } = [];
        public IEnumerable<AttendanceDto> Attendances { get; set; } = [];
    }

    public class CreateGroupDto
    {
        public required string Name { get; set; }
        public required int TeacherId { get; set; }
    }

    public class UpdateGroupDto
    {
        public required string Name { get; set; }
        public required int TeacherId { get; set; }
    }
}
