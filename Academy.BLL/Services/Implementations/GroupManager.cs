using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using AutoMapper;
using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.Services.Implementations
{
    public class GroupManager : CrudManager<Group, GroupDto, CreateGroupDto, UpdateGroupDto>, IGroupService
    {
        public GroupManager(IGroupRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
