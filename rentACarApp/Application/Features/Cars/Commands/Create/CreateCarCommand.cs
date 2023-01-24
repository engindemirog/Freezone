using Application.Features.Cars.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cars.Commands.Create
{
    public class CreateCarCommand:IRequest<CreatedCarResponse>
    {
        public int ModelId { get; set; }
        public int Kilometer { get; set; }
        public short ModelYear { get; set; }
        public string Plate { get; set; }
        public short MinFindeksCreditRate { get; set; }

        public CarState CarState { get; set; }

        public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, CreatedCarResponse>
        {
            private ICarRepository _carRepository;
            private IMapper _mapper;
            private CarBusinessRules _carBusinessRules;

            public CreateCarCommandHandler(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules)
            {
                _carRepository = carRepository;
                _mapper = mapper;
                _carBusinessRules = carBusinessRules;
            }

            public async Task<CreatedCarResponse> Handle(CreateCarCommand request, CancellationToken cancellationToken)
            {
                await _carBusinessRules.EachModelCanContainFiveCars(request.ModelId);

                Car mappedCar = _mapper.Map<Car>(request);

                Car createdCar = await _carRepository.AddAsync(mappedCar);

                CreatedCarResponse createdCarResponse =  _mapper.Map<CreatedCarResponse>(createdCar);

                return createdCarResponse;
            }
        }

    }
}
