using Academy.DAL.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.DTOs
{
    public class GroupDto : Dto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public IEnumerable<(int, string)> Students { get; set; } = [];
    }

    public class CreateGroupDto
    {
        public required string Name { get; set; }
    }

    public class UpdateGroupDto
    {
        public required string Name { get; set; }
    }
}
