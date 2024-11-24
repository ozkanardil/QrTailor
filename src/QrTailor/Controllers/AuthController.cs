using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrTailor.Application.Features.Auth.Models;
using QrTailor.Infrastructure.Results;
using QrTailor.Application.Features.Auth.Queries;
using QrTailor.Application.Features.Auth.Commands;
using System.Security.Claims;

namespace QrTailor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(IRequestDataResult<LoginResponse>))]
        public async Task<IActionResult> PostAsync([FromBody] GetLoginQuery request)
        {
            var res = await _mediator.Send(request);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(IRequestResult))]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserCommand request)
        {
            var res = await _mediator.Send(request);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("getrecoverycode")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(IRequestResult))]
        public async Task<IActionResult> GetRecoveryCode([FromBody] GetRecoveryCodeQuery request)
        {
            var res = await _mediator.Send(request);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("changepassword")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(IRequestResult))]
        public async Task<IActionResult> PostChangePassword([FromBody] ChangePasswordCommand request)
        {
            var res = await _mediator.Send(request);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

        [Authorize(Roles = "User")]
        [HttpGet("getemailverificationcode")]
        [ProducesResponseType(200, Type = typeof(IRequestResult))]
        public async Task<IActionResult> GetEmailVerificationCode([FromQuery] GetEmailVerificationCodeUserResquest request)
        {
            GetEmailVerificationCodeUserQuery query = new GetEmailVerificationCodeUserQuery();
            query.UserId = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var res = await _mediator.Send(query);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

        [Authorize(Roles = "User")]
        [HttpPost("emailverify")]
        [ProducesResponseType(200, Type = typeof(IRequestResult))]
        public async Task<IActionResult> EmailVerify([FromBody] VerifyEmailCommandRequest request)
        {
            VerifyEmailCommand query = new VerifyEmailCommand();
            query.code = request.code;
            query.UserId = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var res = await _mediator.Send(query);
            if (res.Success)
                return Ok(res);

            return BadRequest(res);
        }

    }
}
