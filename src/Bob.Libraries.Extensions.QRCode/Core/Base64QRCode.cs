﻿using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
#if NETSTANDARD2_0
#else
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endif

namespace Bob.Libraries.Extensions.QRCode.Core
{
    public class Base64QRCode : AbstractQRCode<string>, IDisposable
    {
        private QRCode qr;

        public Base64QRCode(QRCodeData data) : base(data) {
            qr = new QRCode(data);
        }
        
        public override string GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }
                

        public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones);
            return BitmapToBase64(bmp, imgType);
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones);
            return BitmapToBase64(bmp, imgType);
        }


        private string BitmapToBase64(Bitmap bmp, ImageType imgType)
        {
            ImageFormat iFormat;
            switch (imgType) {
                case ImageType.Png: 
                    iFormat = ImageFormat.Png;
                    break;
                case ImageType.Jpeg:
                    iFormat = ImageFormat.Jpeg;
                    break;
                case ImageType.Gif:
                    iFormat = ImageFormat.Gif;
                    break;
                default:
                    iFormat = ImageFormat.Png;
                    break;
            }
            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, iFormat);
            byte[] bitmapBytes = memoryStream.GetBuffer();
            string bitmapString = Convert.ToBase64String(bitmapBytes, Base64FormattingOptions.None);
            return bitmapString;
        }

        public enum ImageType
        {
            Gif,
            Jpeg,
            Png
        }

        public void Dispose()
        {
            this.QrCodeData = null;
        }

    }
}
