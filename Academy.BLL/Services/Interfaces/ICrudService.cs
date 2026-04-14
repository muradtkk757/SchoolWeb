using Academy.BLL.DTOs;
using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Academy.BLL.Services.Interfaces
{
    public interface ICrudServiceAsync<TEntity, TGetDto, TCreateDto, TUpdateDto>
     where TEntity : Entity
     where TGetDto : Dto
     where TCreateDto : class
     where TUpdateDto : class
    {
        Task<TGetDto> GetByIdAsync(int id);
        Task<TGetDto> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TGetDto>> GetAllAsync();
        Task<PaginatedResultDto<TGetDto>> GetListAsync(PageRequest pageRequest);    
        Task AddAsync(TCreateDto dto);
        Task UpdateAsync(TUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
