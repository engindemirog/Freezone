using Application.Services.Repositories;
using Domain.Entities;
using Freezone.Core.Application.Rules;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Rules
{
    public class BrandBusinessRules:BaseBusinessRules
    {
        private readonly IBrandRepository _brandRepository;

        public BrandBusinessRules(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task BrandNameCannotBeDuplicatedWhenInserted(string name) 
        {
            Brand? result = await _brandRepository.GetAsync(b=>b.Name==name);
            if (result != null) throw new BusinessException("Brand name already exists");
        }
    }
}
