using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Academy.BLL.Services.Interfaces
{
    public interface IGroupService : ICrudServiceAsync<Group, GroupDto, CreateGroupDto, UpdateGroupDto>
    {
        Task<IEnumerable<GroupDto>> GetAllWithDetailsAsync();
    }
}
