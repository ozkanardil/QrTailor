using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Services.Utils;
using QrTailor.Domain.Entities;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QrTailor.Application.Features.Auth.Queries
{
    public class GetRecoveryCodeQuery : IRequest<IRequestResult>
    {
        public string Email { get; set; }
    }

    public class GetRecoveryCodeQueryHandler : IRequestHandler<GetRecoveryCodeQuery, IRequestResult>
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService _emailService;
        private readonly IUtilsService _utilsService;

        public GetRecoveryCodeQueryHandler(DatabaseContext context, 
                                            IEmailService emailService,
                                            IUtilsService utilsService)
        {
            _context = context;
            _emailService = emailService;
            _utilsService = utilsService;
        }
        public async Task<IRequestResult> Handle(GetRecoveryCodeQuery request, CancellationToken cancellationToken)
        {
            var isUserExists = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

            if (isUserExists == null)
                return new ErrorRequestResult(AuthMessages.UserNotFound);

            isUserExists.RecoveryCode = _utilsService.GenerateRecoveryCode();

            _context.User.Update(isUserExists);
            int resultUpdate = _context.SaveChanges();

            if (resultUpdate == 0)
                return new ErrorRequestResult(AuthMessages.RecoveryCodeGenerationError);

            
            string emailBody = String.Format("Dear {0},<br>" +
                "This is your recovery code: <b>{1}</b><br>" +
                "Please visit the following link and reset your password.<br>" +
                "<a href='https://app.link-map.net/changepassword' targert='_blank'>Reset password link</a><br>", isUserExists.FirstName, isUserExists.RecoveryCode);
            Email email = new Email
            {
                ToMail = isUserExists.Email,
                Body = emailBody,
                Subject = System.String.Format("[{0}] password recovery code", ApplicationSettings.AppName)
            };

            var mailResult = _emailService.MailGonder(email);



            return new SuccessRequestResult(AuthMessages.RecoveryCodeGenerated);
        }
    }
}
