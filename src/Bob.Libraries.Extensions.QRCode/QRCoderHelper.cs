using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using Bob.Libraries.Extensions.QRCode.Core;

namespace Bob.Libraries.Extensions.QRCode
{
    public static class QRCoderHelper
    {
        /// <summary>
        /// 渲染二维码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Bitmap RenderQrCode(string str, string logoPath = null, bool drawQuietZones = true)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q))
                {
                    using (Core.QRCode qrCode = new Core.QRCode(qrCodeData))
                    {
                        if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                        {
                            return qrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Image.FromFile(logoPath),
                                drawQuietZones: drawQuietZones);
                        }
                        else
                        {
                            return qrCode.GetGraphic(20, Color.Black, Color.White, drawQuietZones);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 改变图片大小
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bitmap Resize(this Bitmap bitmap, int size)
        {
            return new Bitmap(bitmap, size, size);
        }
        /// <summary>
        /// 转换为Byte数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
