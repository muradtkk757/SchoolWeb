using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.Services.Interfaces
{
    public interface IGroupService : ICrudServiceAsync<Group, GroupDto, CreateGroupDto, UpdateGroupDto>
    {
    }
}
