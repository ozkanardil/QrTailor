using AutoMapper;
using MediatR;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using QrTailor.Infrastructure.Results;

namespace QrTailor.Application.Features.QrCodeGenerator.Queries
{
    public class GetQrCodeQuery : IRequest<IRequestDataResult<string>>
    {
        public string? qrCodeLink { get; set; }
        public string? staticToken { get; set; }
    }

    public class GetQrCodeQueryHandler : IRequestHandler<GetQrCodeQuery, IRequestDataResult<string>>
    {
        private readonly IMapper _mapper;

        public GetQrCodeQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<IRequestDataResult<string>> Handle(GetQrCodeQuery request, CancellationToken cancellationToken)
        {

            #region QrCode_generatoion_code
            string data = string.Empty;
            var guid = Guid.NewGuid().ToString();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(request.qrCodeLink + "\r\n" + guid, QRCodeGenerator.ECCLevel.Q);
            QrCodeManager qrCode = new QrCodeManager(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            // use this when you want to show your logo in middle of QR Code and change color of qr code
            //Bitmap logoImage = new Bitmap(@"wwwroot/img/aircodlogo.jpg");
            // Generate QR Code bitmap and convert to Base64
            //using (Bitmap qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.WhiteSmoke, logoImage))
            using (Bitmap qrCodeAsBitmap = qrCode.GetGraphic(20, Color.Black, Color.WhiteSmoke, null))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeAsBitmap.Save(ms, ImageFormat.Png);
                    string base64String = Convert.ToBase64String(ms.ToArray());

                    if (base64String == null)
                        return new ErrorRequestDataResult<string>(null, "Code generation error");

                    data = "data:image/png;base64," + base64String;
                }
            }
            #endregion QrCode_generatoion_code

            return new SuccessRequestDataResult<string>(data.ToString(), "Qr code tamam");

        }
    }
}
