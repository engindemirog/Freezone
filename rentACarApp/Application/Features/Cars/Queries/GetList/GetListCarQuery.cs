using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cars.Queries.GetList
{
    public class GetListCarQuery : IRequest<GetListResponse<GetListCarDto>>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListCarQueryHandler : IRequestHandler<GetListCarQuery, GetListResponse<GetListCarDto>>
        {
            private ICarRepository _carRepository;
            private IMapper _mapper;

            public GetListCarQueryHandler(ICarRepository carRepository, IMapper mapper)
            {
                _carRepository = carRepository;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetListCarDto>> Handle(GetListCarQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Car> cars = await _carRepository.GetListAsync(c => c.CarState != CarState.Maintenance,
                                                                        include:
                                                                        c => c.Include(c => c.Model)
                                                                              .Include(c => c.Model.Brand),
                                                                        index: request.PageRequest.Page,
                                                                        size: request.PageRequest.PageSize
                                                                             );
                GetListResponse<GetListCarDto> response = _mapper.Map<GetListResponse<GetListCarDto>>(cars);

                return response;
            }
        }
    }
}
