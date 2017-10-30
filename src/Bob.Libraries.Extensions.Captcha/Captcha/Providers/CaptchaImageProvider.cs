using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using Bob.Libraries.Extensions.Captcha.Contracts;

namespace Bob.Libraries.Extensions.Captcha.Providers
{
    /// <summary>
    /// The default captcha image provider
    /// </summary>
    public class CaptchaImageProvider : ICaptchaImageProvider
    {
        /// <summary>  
        /// 该方法是将生成的随机数写入图像文件  
        /// </summary>  
        /// <param name="code">code是一个随机数</param>
        public byte[] DrawCaptcha(string code, float emSize, int imageWidth, int imageHeight)
        {
            Random random = new Random();
            //验证码颜色集合  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };


            //定义图像的大小，生成图像的实例  
            using (Bitmap Img = new Bitmap(imageWidth, imageHeight))
            {
                //从Img对象生成新的Graphics对象    
                using (Graphics g = Graphics.FromImage(Img))
                {
                    g.Clear(Color.White);//背景设为白色  

                    //在随机位置画背景点  
                    for (int i = 0; i < 100; i++)
                    {
                        int x = random.Next(Img.Width);
                        int y = random.Next(Img.Height);
                        g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
                    }
                    //验证码绘制在g中  
                    for (int i = 0; i < code.Length; i++)
                    {
                        int cindex = random.Next(7);//随机颜色索引值  
                        int findex = random.Next(5);//随机字体索引值  
                        Font f = new Font(fonts[findex], emSize, FontStyle.Bold);//字体  
                        Brush b = new SolidBrush(c[cindex]);//颜色  
                        int ii = 4;
                        if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                        {
                            ii = 2;
                        }
                        g.DrawString(code.Substring(i, 1), f, b, random.Next(i * (int)emSize - 5, i * (int)emSize), ii);//绘制一个验证字符  
                    }
                    using (var stream = new MemoryStream())
                    {
                        Img.Save(stream, ImageFormat.Png);
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}