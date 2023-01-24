using Domain.Enums;

namespace Application.Features.Cars.Commands.Create;

public class CreatedCarResponse
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public int Kilometer { get; set; }
    public short ModelYear { get; set; }
    public string Plate { get; set; }
    public short MinFindeksCreditRate { get; set; }
    public CarState CarState { get; set; }
}