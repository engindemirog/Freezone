using Application.Features.Cars.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Dynamic;
using Freezone.Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cars.Queries.GetListByDynamic;

public class GetListCarByDynamicQuery : IRequest<GetListResponse<GetListCarByDynamicDto>>
{
    public PageRequest PageRequest { get; set; }
    public Dynamic Dynamic { get; set; }

    public class GetListCarByDynamicQueryHandler : IRequestHandler<GetListCarByDynamicQuery, GetListResponse<GetListCarByDynamicDto>>
    {
        private ICarRepository _carRepository;
        private IMapper _mapper;

        public GetListCarByDynamicQueryHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
        public async Task<GetListResponse<GetListCarByDynamicDto>> Handle(GetListCarByDynamicQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Car> cars = await _carRepository.GetListByDynamicAsync(dynamic: request.Dynamic,
                                                                             include: c => c.Include(c => c.Model).Include(c => c.Model.Brand),
                                                                             index: request.PageRequest.Page,
                                                                             size : request.PageRequest.PageSize);
            GetListResponse<GetListCarByDynamicDto> response = _mapper.Map<GetListResponse<GetListCarByDynamicDto>>(cars);

            return response;
        }
    }
}
