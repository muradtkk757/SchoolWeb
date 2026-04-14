using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Jwt
{
    public interface ITokenService
    {
        string GetToken(AppUser appUser, IList<string> roles);
    }
}
