using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Features.Auth.Constants
{
    public static class AuthMessages
    {
        public static string UserNotFound = "User not found.";
        public static string UserNotApproved = "User not approved.";
        public static string UserAccountSuspended = "User account suspended.";
        public static string UserLoginOk = "Login succesfull.";
        public static string UserLoginErr = "Login error.";
        public static string RecoveryCodeGenerationError = "Recovery code generating error. Please contact with support team";
        public static string RecoveryCodeGenerated = "Recovery code generation is succesfull.";
        public static string InvalidCode = "Invalid code.";
        public static string UnmatchedPassword = "Paswords are unmatched.";
        public static string PasswordsUpdateFailed = "Pasword update failed.";
        public static string PasswordsChanged = "Pasword has been changed.";
        public static string EmailVerificationFailed = "Email verification failed.";
        public static string EmailVerified = "Email verified.";

        public static string ThisEmailInUse = "This email is in use";

        public static string UserCreateSuccess = "User has been created.";
        public static string UserCreateError = "User can not be created.";
        public static string RequestCanNotBeNull = "Value cannot be null.";
        public static string GuardStaticTokenIsNotValid = "Invalid request.";
    }
}
