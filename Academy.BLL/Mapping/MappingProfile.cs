using Academy.BLL.DTOs;
using Academy.DAL.DataContext.Entities;
using AutoMapper;
using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<CreateStudentDto, Student>().ReverseMap();
            CreateMap<UpdateStudentDto, Student>().ReverseMap();
            CreateMap<PaginatedResult<Student>, PaginatedResultDto<StudentDto>>();

            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<CreateGroupDto, Group>().ReverseMap();
            CreateMap<UpdateGroupDto, GroupDto>().ReverseMap();
            CreateMap<PaginatedResult<Group>, PaginatedResultDto<GroupDto>>();
        }
    }
}
