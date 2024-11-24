using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QrTailor.Application.Features.Role.Constants;
using QrTailor.Application.Features.Role.Models;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;

namespace QrTailor.Application.Features.Role.Queries
{
    public class GetRoleQuery : IRequest<IRequestDataResult<IEnumerable<RoleResponse>>>
    {
    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, IRequestDataResult<IEnumerable<RoleResponse>>>
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public GetRoleQueryHandler(IMapper mapper, DatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IRequestDataResult<IEnumerable<RoleResponse>>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Role.ToListAsync();
            var response = _mapper.Map<IEnumerable<RoleResponse>>(result);

            if (!response.Any())
                return new ErrorRequestDataResult<IEnumerable<RoleResponse>>(response, Messages.RolesListError);

            return new SuccessRequestDataResult<IEnumerable<RoleResponse>>(response, Messages.RolesListed); ;
        }
    }
}
