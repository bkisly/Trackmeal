using System.Drawing;
using QRCoder;
using Trackmeal.Models;

namespace Trackmeal.ViewModels
{
    public class OrderSummaryViewModel
    {
        public Order Order { get; set; } = null!;
        //public string OrderStatusUrl { get; private set; } = string.Empty;
        public string QrCodeImageSource { get; }

        public OrderSummaryViewModel(string orderStatusUrl)
        {
            QrCodeImageSource = GenerateQrCodeImageSource(orderStatusUrl);
        }

        private static string GenerateQrCodeImageSource(string content)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrData = Convert.ToBase64String(qrCode.GetGraphic(20));
            return $"data:image/png;base64,{qrData}";
        }
    }
}
