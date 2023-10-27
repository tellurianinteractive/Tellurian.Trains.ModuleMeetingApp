using QRCoder;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

internal static class QrCoderExtensions
{
    public static string QRCode(this string text)
    {
        var png = text.AsPng();
        return string.Format("data:image/pgn;base64,{0}", Convert.ToBase64String(png));
    }

    private static byte[] AsPng(this string data)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        return qrCode.GetGraphic(40);
    }
}
