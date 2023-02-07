using Application.Features.Auth.Constants;
using Application.Services.Repositories;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using Freezone.Core.Security.Entities;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules
{
    private IUserRepository _userRepository;

    public AuthBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task UserEmailCannotBeDuplicatedWhenInserted(string email)
    {
        User? user = await _userRepository.GetAsync(u=>u.Email == email);
        if (user != null)
        {
            throw new BusinessException(AuthBusinessMessages.UserEmailAlreadyExists);
        }
    }
}