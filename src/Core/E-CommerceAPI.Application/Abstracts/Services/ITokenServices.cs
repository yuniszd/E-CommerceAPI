﻿using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface ITokenService
{
    string GenerateJwtToken(AppUser user, IList<string> roles);
    RefreshToken GenerateRefreshToken(string userId);
}
