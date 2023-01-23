using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries;

public class GetListModelQuery : IRequest<GetListResponse<GetListModelDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListModelQueryHandler : IRequestHandler<GetListModelQuery, GetListResponse<GetListModelDto>>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public GetListModelQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListModelDto>> Handle(GetListModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Model> models = await _modelRepository.GetListAsync(include:
                                                                          c => c.Include(c => c.Brand)
                                                                              .Include(c => c.Fuel)
                                                                              .Include(c => c.Transmission),
                                                                          index: request.PageRequest.Page,
                                                                          size: request.PageRequest.PageSize
                                      );
            GetListResponse<GetListModelDto> response = _mapper.Map<GetListResponse<GetListModelDto>>(models);
            return response;
        }
    }
}
