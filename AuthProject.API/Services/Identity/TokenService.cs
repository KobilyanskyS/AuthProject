﻿using AuthProject.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using AuthProject.API.Extensions;

namespace AuthProject.API.Services.Identity;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(ApplicationUser user, List<IdentityRole<long>> roles)
    {
        var token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
        var tokenHandler = new JwtSecurityTokenHandler();


        return tokenHandler.WriteToken(token);
    }
}
