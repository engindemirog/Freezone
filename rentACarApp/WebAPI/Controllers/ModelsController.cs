using Application.Features.Models.Queries;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Dynamic;
using Freezone.Core.Persistence.Paging;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelsController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListModelQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListModelDto> result = await Mediator.Send(getListModelQuery);
        return Ok(result);
    }

    
}
