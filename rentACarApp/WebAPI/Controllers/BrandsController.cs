using Application.Features.Brands.Commands.Create;
using Application.Features.Brands.Queries.GetById;
using Application.Features.Brands.Queries.GetList;
using Freezone.Core.Application.Requests;
using Freezone.Core.Persistence.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand) 
        {
            CreatedBrandResponse response = await Mediator.Send(createBrandCommand);

            return Created("",response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            GetByIdBrandResponse response = await Mediator.Send(new GetByIdBrandQuery {Id=id });
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
            GetListResponse<GetListBrandDto> response = await Mediator.Send(getListBrandQuery);
            return Ok(response);
        }
    }
}
