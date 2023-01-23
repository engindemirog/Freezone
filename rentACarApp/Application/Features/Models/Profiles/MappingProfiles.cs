using Application.Features.Models.Queries;
using AutoMapper;
using Domain.Entities;
using Freezone.Core.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Model, GetListModelDto>().ForMember(c => c.BrandName, opt => opt.MapFrom(c => c.Brand.Name))
                                        .ForMember(c => c.FuelName, opt => opt.MapFrom(c => c.Fuel.Name))
                                        .ForMember(c => c.TransmissionName,
                                                   opt => opt.MapFrom(c => c.Transmission.Name));
        CreateMap<IPaginate<Model>, GetListResponse<GetListModelDto>>().ReverseMap();
    }
}