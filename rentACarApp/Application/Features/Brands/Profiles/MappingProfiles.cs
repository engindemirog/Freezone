using Application.Features.Brands.Commands.Create;
using Application.Features.Brands.Queries.GetById;
using Application.Features.Brands.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Freezone.Core.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Profiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Brand, CreateBrandCommand>().ReverseMap();
            CreateMap<Brand, CreatedBrandResponse>().ReverseMap();
            CreateMap<Brand, GetByIdBrandResponse>().ReverseMap();
            CreateMap<Brand, GetListBrandDto>().ReverseMap();
            CreateMap<IPaginate<Brand>, GetListResponse<GetListBrandDto>>().ReverseMap();
        }
    }
}
