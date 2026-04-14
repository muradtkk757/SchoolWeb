using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Core.Application.Jwt;
using Core.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Academy.BLL.Services.Implementations
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthManager(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
                throw new Exception("Invalid credentials");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            return _tokenService.GetToken(user, roles);
        }
    }
}
