using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Application.Features.Auth.Models;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using QrTailor.Infrastructure.Security.JwtToken;
using QrTailor.Infrastructure.Security.Hashing;
using QrTailor.Application.Features.UserRole.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Services.Shared;
using QrTailor.Domain.Entities;

namespace QrTailor.Application.Features.Auth.Queries
{
    public class GetLoginQuery : IRequest<IRequestDataResult<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class GetLoginQueryHandler : IRequestHandler<GetLoginQuery, IRequestDataResult<LoginResponse>>
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private ITokenHelper _tokenHelper;
        private IEmailService _emailService;

        public GetLoginQueryHandler(IMapper mapper, DatabaseContext context, ITokenHelper tokenHelper, IEmailService emailService)
        {
            _mapper = mapper;
            _context = context;
            _tokenHelper = tokenHelper;
            _emailService = emailService;
        }
        public async Task<IRequestDataResult<LoginResponse>> Handle(GetLoginQuery request, CancellationToken cancellationToken)
        {
            var userToCheck = _context.User.FirstOrDefault(u => u.Email == request.Email);

            if (userToCheck == null || userToCheck.Status == 3)
                return new ErrorRequestDataResult<LoginResponse>(null, AuthMessages.UserNotFound);

            DateTime today = DateTime.Today;
            TimeSpan difference = userToCheck.CreateDate - today;
            int dateDifference = (int)difference.TotalDays + ConstantsShared.EmailActivationPeriod;

            if (userToCheck.Status == 0 && dateDifference < 1)
            {
                userToCheck.Status = 2;
                _context.User.Update(userToCheck);
                _context.SaveChanges();

                string emailBody = System.String.Format("Dear {0} {1},<br>" +
               "Your account has been suspended because you have not verified your email address for more than 30 days. <br>" +
               "Please visit the following link and reset your password. Or, contact with Link Map team using contact page.<br>" +
               "<a href='https://app.link-map.net/forgetpassword' targert='_blank'>Reset password link</a><br>", userToCheck.FirstName, userToCheck.LastName);
                Email email = new Email
                {
                    ToMail = userToCheck.Email,
                    Body = emailBody,
                    Subject = System.String.Format("[{0}] Your Link-Map accout is suspended", ApplicationSettings.AppName)
                };

                var mailResult = _emailService.MailGonder(email);

                return new ErrorRequestDataResult<LoginResponse>(null, AuthMessages.UserAccountSuspended);
            }

            if (userToCheck.Status == 2)
                return new ErrorRequestDataResult<LoginResponse>(null, AuthMessages.UserAccountSuspended);

            if (!HashingHelper.VerifyPasswordHash(request.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
                return new ErrorRequestDataResult<LoginResponse>(null, AuthMessages.UserLoginErr);

            var userClaims = await _context.UserRole.Where(ur => ur.UserId == userToCheck.Id).Include(r => r.Role).ToListAsync();
            var claims = _mapper.Map<List<UserRoleDto>>(userClaims);

            var token = _tokenHelper.CreateToken(userToCheck, claims);

            var userVClaims = await _context.UserRoleV.Where(ur => ur.UserId == userToCheck.Id).ToListAsync();
            foreach (var item in userVClaims)
            {
                item.OperationName = item.OperationName.Trim();
            }

            TokenResult tokenResult = new TokenResult();
            tokenResult.Token = token.Token;
            tokenResult.Expiration = token.Expiration;
            tokenResult.refreshToken = "";

            LoginResponse result = new LoginResponse();
            result.Token = tokenResult;
            result.Roles = _mapper.Map<List<UserRoleResponse>>(userVClaims);

            if (token == null)
                return new ErrorRequestDataResult<LoginResponse>(result, AuthMessages.UserLoginErr);

            return new SuccessRequestDataResult<LoginResponse>(result, AuthMessages.UserLoginOk);
        }
    }
}
