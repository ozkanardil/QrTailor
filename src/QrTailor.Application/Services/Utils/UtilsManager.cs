
namespace QrTailor.Application.Services.Utils
{
    public class UtilsManager : IUtilsService
    {
        public int GenerateRecoveryCode()
        {
            Random random = new();
            int sixDigitRecoveryCode = random.Next(100000, 1000000);
            return sixDigitRecoveryCode;
        }
    }
}
