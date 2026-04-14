using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using AutoMapper;
using Core.Persistence.Models;
using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Academy.BLL.Services.Implementations
{
    public class CrudManager<TEntity, TGetDto, TCreateDto, TUpdateDto> : ICrudServiceAsync<TEntity, TGetDto, TCreateDto, TUpdateDto>
      where TEntity : Entity
      where TGetDto : Dto
      where TCreateDto : class
      where TUpdateDto : class
    {
        private readonly IRepositoryAsync<TEntity, AcademyDbContext> _repository;
        private readonly IMapper _mapper;

        public CrudManager(IRepositoryAsync<TEntity, AcademyDbContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception($"Entity with id {id} not found.");
            }

            await _repository.DeleteAsync(entity);
        }

        public async Task<IEnumerable<TGetDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TGetDto>>(entities);

            return dtos;
        }

        public async Task<TGetDto> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _repository.GetAsync(predicate);
            var dto = _mapper.Map<TGetDto>(entity);

            return dto;
        }

        public async Task<TGetDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            var dto = _mapper.Map<TGetDto>(entity);

            return dto;
        }

        public async Task<PaginatedResultDto<TGetDto>> GetListAsync(PageRequest pageRequest)
        {
            var paginatedResult = await _repository.GetListAsync(pageRequest);
            var dto = _mapper.Map<PaginatedResultDto<TGetDto>>(paginatedResult);

            return dto;
        }

        public async Task UpdateAsync(TUpdateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            await _repository.UpdateAsync(entity);
        }
    }
}
