using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using QrTailor.Infrastructure.Results;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace QrTailor.Application.Features.QrCodeGenerator.Queries
{

    public class GetQrCodeMazeQuery : IRequest<IRequestDataResult<string>>
    {
        public string? qrCodeLink { get; set; }
        public string? staticToken { get; set; }
    }

    public class GetQrCodeMazeQueryHandler : IRequestHandler<GetQrCodeMazeQuery, IRequestDataResult<string>>
    {
        private readonly QRCodeGenerator qrGenerator = new();
        public GetQrCodeMazeQueryHandler()
        {
        }
        public async Task<IRequestDataResult<string>> Handle(GetQrCodeMazeQuery request, CancellationToken cancellationToken)
        {

            //var qrCodeData = qrGenerator.CreateQrCode(new Url("https://www.code-maze.com"));
            var qrCodeData = qrGenerator.CreateQrCode(new Url("https://qrtailor.vercel.app/"));

            var result = GeneratePng(qrCodeData);

            return new SuccessRequestDataResult<string>(result, "Qr code tamam");

        }

        private static string GeneratePng(QRCodeData data)
        {
            using var qrCode = new PngByteQRCode(data);
            //var qrCodeImage = qrCode.GetGraphic(20, [255, 0, 0], [0, 0, 139]);
            var qrCodeImage = qrCode.GetGraphic(20, Color.Blue, Color.WhiteSmoke);
            //var qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.WhiteSmoke);

            return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
        }
    }


}
