using Application.Features.Cars.Commands.Create;
using Application.Features.Cars.Queries.GetList;
using Application.Features.Cars.Queries.GetListByDynamic;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Dynamic;
using Freezone.Core.Persistence.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] CreateCarCommand createCarCommand)
    {
        CreatedCarResponse response = await Mediator.Send(createCarCommand);
        return Created("", response);
    }

    [HttpGet]
    public async Task<ActionResult> GetList([FromQuery]PageRequest pageRequest) 
    {
        GetListResponse<GetListCarDto> result =  await Mediator.Send(new GetListCarQuery {PageRequest=pageRequest});
        return Ok(result);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<ActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic=null)
    {
        GetListCarByDynamicQuery getListCarByDynamicQuery = new() { PageRequest=pageRequest, Dynamic = dynamic};
        GetListResponse<GetListCarByDynamicDto> result = await Mediator.Send(getListCarByDynamicQuery);
        return Ok(result);
    }
}
