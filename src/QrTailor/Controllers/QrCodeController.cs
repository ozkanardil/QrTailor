using QrTailor.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrTailor.Application.Features.QrCodeGenerator.Queries;

namespace QrTailor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QrCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }
      
        [AllowAnonymous]
        [HttpGet("GenerateQrCodeMaze")]
        [ProducesResponseType(200, Type = typeof(IRequestDataResult<string>))]
        public async Task<IActionResult> GetCreateQrCodeMaze([FromQuery] GetQrCodeMazeQuery request)
        {
            var res = await _mediator.Send(request);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }
    }
}


