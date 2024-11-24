using AutoMapper;
using MediatR;
using QrTailor.Application.Features.Auth.Constants;
using QrTailor.Application.Features.Auth.Rules;
using QrTailor.Infrastructure.Results;
using QrTailor.Persistance.Context;
using System.Security.Cryptography;
using QrTailor.Infrastructure.Security.Hashing;
using QrTailor.Domain.Entities;
using QrTailor.Application.Features.Auth.Models;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Services.Utils;

namespace QrTailor.Application.Features.Auth.Commands
{
    public class CreateUserCommand : IRequest<IRequestResult>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IRequestResult>
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService _emailService;
        private readonly IUtilsService _utilsService;

        public CreateUserCommandHandler(IEmailService emailService,
                                        IUtilsService utilsService,
                                        DatabaseContext context)
        {
            _emailService = emailService;
            _context = context;
            _utilsService = utilsService;
        }
        public async Task<IRequestResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userToCheck = _context.User.FirstOrDefault(u => u.Email == request.email);

            if (userToCheck != null)
                return new ErrorRequestDataResult<LoginResponse>(null, AuthMessages.ThisEmailInUse);

            HashingHelper.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSaltHash);

            UserEntity userEntity = new UserEntity();
            userEntity.FirstName = request.firstName;
            userEntity.LastName = request.lastName;
            userEntity.NickName = "-";
            userEntity.Status = 0;
            userEntity.Email = request.email;
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSaltHash;
            userEntity.RecoveryCode = _utilsService.GenerateRecoveryCode();
            userEntity.RefreshTokenA = "-";
            userEntity.CreateDate = DateTime.UtcNow;

            _context.User.Add(userEntity);
            int userCreateResult = _context.SaveChanges();

            if (userCreateResult == 0)
                return new ErrorRequestDataResult<string>(String.Empty, AuthMessages.UserCreateError);

            int newUserId = userEntity.Id;
            UserRoleEntity userRolEntity = new UserRoleEntity();
            userRolEntity.UserId = newUserId;
            userRolEntity.OperationClaimId = 2;

            // TODO: Burada kullanıcı ile birlikte Rol oluturulamaması halinde mana mail gelmeli.
            _context.UserRole.Add(userRolEntity);
            int userRoleCreateResult = _context.SaveChanges();

            //INFO mail to new user
            string userEmailBody = String.Format("Dear {0} {1},<br>" +
            "You must activate your account using the activation code below.<br>" +
            "To activate your account, login to your account.<br>" +
            "<b>Account activation code:</b> {2} <br>" +
            "<a href='https://app.link-map.net/login' target='_blank'>Click here to login</a>" +
            "Link Map Team<br>", userEntity.FirstName, userEntity.LastName, userEntity.RecoveryCode);
            Email userEmail = new Email
            {
                ToMail = userEntity.Email,
                Body = userEmailBody,
                Subject = String.Format("[{0}] Link Map Account Activation.", ApplicationSettings.AppName)
            };
            var userEmailResult = _emailService.MailGonder(userEmail);

            // Info mail to ADMIN
            string adminEmailBody = String.Format("{0} {1} named a new user has been registered.", userEntity.FirstName, userEntity.LastName);
            Email adminEmail = new Email
            {
                ToMail = "ozkanardil@gmail.com",
                Body = adminEmailBody,
                Subject = String.Format("[{0}] New User Registration", ApplicationSettings.AppName)
            };
            var adminEmailResult = _emailService.MailGonder(adminEmail);

            return new SuccessRequestResult(AuthMessages.UserCreateSuccess);
        }

    }


}
