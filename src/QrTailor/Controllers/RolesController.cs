﻿using ArticleMap.Application.Features.Role.Models;
using ArticleMap.Application.Features.Role.Queries;
using ArticleMap.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticleMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IRequestDataResult<IEnumerable<RoleResponse>>))]
        public async Task<IActionResult> GetAsync([FromQuery] GetRoleQuery request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }
    }
}