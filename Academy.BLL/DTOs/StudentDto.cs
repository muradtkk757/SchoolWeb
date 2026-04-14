using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class StudentDto : Dto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? GroupName { get; set; }
    }

    public class CreateStudentDto
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
    }

    public class UpdateStudentDto
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
    }
}
