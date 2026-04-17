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
using System.Threading.Tasks;

namespace Academy.BLL.Services.Implementations
{
    public class GroupManager : CrudManager<Group, GroupDto, CreateGroupDto, UpdateGroupDto>, IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IMapper _mapper;

        public GroupManager(IGroupRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> GetAllWithDetailsAsync()
        {
            var entities = await _repository.GetAllAsync(x => x.Teacher, x => x.Students);
            var dtos = _mapper.Map<IEnumerable<GroupDto>>(entities);
            return dtos;
        }
    }
}
