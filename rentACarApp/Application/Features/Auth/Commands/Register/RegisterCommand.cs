using Application.Features.Auth.Rules;
using Freezone.Core.Application.Dtos;
using Freezone.Core.Security.JWT;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;

        public RegisterCommandHandler(AuthBusinessRules authBusinessRules)
        {
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
           await _authBusinessRules.UserEmailCannotBeDuplicatedWhenInserted(request.UserForRegisterDto.Email);

            //Todo: Hashing

            //Todo: Insert User

            //Todo: Generate AccessToken
        }
    }
}

public class RegisteredResponse
{
    public AccessToken AccessToken { get; set; }
}