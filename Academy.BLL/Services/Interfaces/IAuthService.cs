using Academy.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
