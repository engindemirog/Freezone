using Application.Features.Auth.Commands.Revoke;
using AutoMapper;
using Freezone.Core.Application.Dtos;
using Freezone.Core.Security.Entities;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserForRegisterDto>().ReverseMap();
        CreateMap<RefreshToken, RevokedResponse>().ReverseMap();
    }
}