using AutoMapper;
using MediatR;
using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using QrTailor.Infrastructure.Security.Hashing;
using QrTailor.Application.Services.EmailService;
using QrTailor.Domain.Entities;

namespace QrTailor.Application.Features.Auth.Commands
{
    public class ChangePasswordCommand : IRequest<IRequestResult>
    {
        public int code { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordConfirmation { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IRequestResult>
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService _emailService;

        public ChangePasswordCommandHandler(IEmailService emailService,
                                        DatabaseContext context)
        {
            _emailService = emailService;
            _context = context;
        }
        public async Task<IRequestResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userToCheck = _context.User.FirstOrDefault(u => u.RecoveryCode == request.code && u.Email == request.email);

            if (userToCheck == null)
                return new ErrorRequestResult(AuthMessages.InvalidCode);

            if (request.password != request.passwordConfirmation)
                return new ErrorRequestResult(AuthMessages.UnmatchedPassword);

            HashingHelper.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSaltHash);

            userToCheck.RecoveryCode = 0;
            userToCheck.Status = 1;
            userToCheck.PasswordHash = passwordHash;
            userToCheck.PasswordSalt = passwordSaltHash;

            _context.User.Update(userToCheck);
            int userCreateResult = _context.SaveChanges();

            if (userCreateResult == 0)
                return new ErrorRequestResult(AuthMessages.PasswordsUpdateFailed);


            string emailBody = String.Format("Dear {0},<br>" +
               "Your password has been changed.<br>" +
               "If you did not change your password, please contact with us.<br>" +
               "Link Map Team<br>", userToCheck.FirstName);

            Email email = new Email
            {
                ToMail = userToCheck.Email,
                Body = emailBody,
                Subject = String.Format("[{0}] You password has been changed.", ApplicationSettings.AppName) 
            };

            var mailResult = _emailService.MailGonder(email);

            return new SuccessRequestResult(AuthMessages.PasswordsChanged);
        }

    }


}
