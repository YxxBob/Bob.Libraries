using System;
using Bob.Libraries.Extensions.Captcha.Contracts;
using Microsoft.AspNetCore.Http;

namespace Bob.Libraries.Extensions.Captcha.Providers
{
    public class CaptchaCodeMain : ICaptchaCodeMain
    {
        private readonly ICaptchaImageProvider _captchaImageProvider;
        private readonly ICaptchaStorageProvider _captchaStorageProvider;
        private readonly ICaptchaCodeGenerator _captchaCodeGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CaptchaCodeMain(
            ICaptchaImageProvider captchaImageProvider,
            ICaptchaStorageProvider captchaStorageProvider,
            ICaptchaCodeGenerator captchaCodeGenerator, 
            IHttpContextAccessor httpContextAccessor)
        {
            _captchaImageProvider = captchaImageProvider;
            _captchaStorageProvider = captchaStorageProvider;
            _captchaCodeGenerator = captchaCodeGenerator;
            _httpContextAccessor = httpContextAccessor;
        }


        public byte[] GeneratorCaptcha(string name,int captchaLength,bool hasNumber,bool haslower,bool hasUpper, int imageWidth,int imageHeight, float fontSize)
        {
            var text = _captchaCodeGenerator.OutputText(haslower, hasUpper, hasNumber, captchaLength);
            _captchaStorageProvider.Add(_httpContextAccessor.HttpContext, name, text);
            byte[] image = null;
            try
            {
                image = _captchaImageProvider.DrawCaptcha(text, fontSize, imageWidth,imageHeight);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return image;
        }

        public void Remove(string name)
        {
            _captchaStorageProvider.Remove(_httpContextAccessor.HttpContext, name);
        }

        public bool VerifyCaptcha(string name, string value)
        {
            var isValid = false;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                //validate request
                var captchaText = _captchaStorageProvider.GetValue(_httpContextAccessor.HttpContext, name);
                if (value.Equals(captchaText, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                    Remove(name);
                }
            }
            return isValid;
        }
    }
}