using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Freezone.Core.Application.Dtos;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.Hashing;
using Freezone.Core.Security.JWT;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IMapper _mapper; // Ctrl + .
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(AuthBusinessRules authBusinessRules, IMapper mapper,
                                      IUserRepository userRepository, IAuthService authService)
        {
            _authBusinessRules = authBusinessRules;
            _mapper = mapper;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailCannotBeDuplicatedWhenInserted(request.UserForRegisterDto.Email);

            // Hashing
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);
            // Insert User
            User newUser = _mapper.Map<User>(request.UserForRegisterDto);
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            newUser.Status = true;
            await _userRepository.AddAsync(newUser);

            // Generate AccessToken
            AccessToken createdAccessToken = await _authService.CreateAccessToken(newUser);
            
            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(newUser, request.IpAddress);
            await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredResponse response = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = createdRefreshToken
            };
            return response;
        }
    }
}