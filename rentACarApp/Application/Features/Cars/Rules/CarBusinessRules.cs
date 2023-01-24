using Application.Services.Repositories;
using Freezone.Core.Application.Rules;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cars.Rules
{
    public class CarBusinessRules:BaseBusinessRules
    {
        private ICarRepository _carRepository;

        public CarBusinessRules(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task EachModelCanContainFiveCars(int modelId) 
        {
            var result = await _carRepository.GetListAsync(c=>c.ModelId == modelId);

            if (result.Count>=5)
            {
                throw new BusinessException("Each Model Can Contain Five Cars");
            }

        }
    }
}
