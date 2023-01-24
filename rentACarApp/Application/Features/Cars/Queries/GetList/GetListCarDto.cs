using Domain.Enums;

namespace Application.Features.Cars.Queries.GetList
{
    public class GetListCarDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Plate { get; set; }
        public CarState CarState { get; set; }
        public int ModelYear { get; set; }

    }
}