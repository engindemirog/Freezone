using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Freezone.Core.Application.Pipelines.Caching;
using Freezone.Core.Application.Pipelines.Logging;
using Freezone.Core.Application.Pipelines.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Brands.Constants;
using Freezone.Core.Application.Pipelines.Authorization;

namespace Application.Features.Brands.Commands.Create;

public class CreateBrandCommand:IRequest<CreatedBrandResponse>,ILoggableRequest,ICacheRemoverRequest,ISecuredOperation
{
    public string Name { get; set; }
    
    public bool BypassCache { get; }
    public string CacheKey => "GetListBrand";
    public string[] Roles => new string[] { BrandsRoles.Create };

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {

        IMapper _mapper;
        IBrandRepository _brandRepository;
        BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IMapper mapper, IBrandRepository brandRepository, BrandBusinessRules brandBusinessRules)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

            Brand mappedBrand = _mapper.Map<Brand>(request);

            _brandRepository.Add(mappedBrand);
            //_brandRepository.Add(mappedBrand);

            CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(mappedBrand);

            return response;

        }
    }

}


