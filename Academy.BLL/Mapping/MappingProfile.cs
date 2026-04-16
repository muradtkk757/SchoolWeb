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
            CreateMap<Student, GroupStudentDto>();
            CreateMap<CreateStudentDto, Student>().ReverseMap();
            CreateMap<UpdateStudentDto, Student>().ReverseMap();
            CreateMap<PaginatedResult<Student>, PaginatedResultDto<StudentDto>>();

            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<CreateTeacherDto, Teacher>().ReverseMap();
            CreateMap<UpdateTeacherDto, Teacher>().ReverseMap();
            CreateMap<PaginatedResult<Teacher>, PaginatedResultDto<TeacherDto>>();

            CreateMap<Attendance, AttendanceDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Student.Group.Name))
                .ReverseMap();
            CreateMap<CreateAttendanceDto, Attendance>().ReverseMap();
            CreateMap<UpdateAttendanceDto, Attendance>().ReverseMap();
             CreateMap<PaginatedResult<Attendance>, PaginatedResultDto<AttendanceDto>>();

            CreateMap<Group, GroupDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : null))
                //.ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
                .ReverseMap();
            CreateMap<CreateGroupDto, Group>().ReverseMap();
            CreateMap<UpdateGroupDto, GroupDto>().ReverseMap();
            CreateMap<PaginatedResult<Group>, PaginatedResultDto<GroupDto>>();
        }
    }
}
