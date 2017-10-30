namespace Bob.Libraries.Extensions.Captcha.Contracts
{
    public interface ICaptchaCodeMain
    {
        byte[] GeneratorCaptcha(string name, int captchaLength, bool hasNumber, bool haslower, bool hasUpper, int imageWidth, int imageHeight, float fontSize);

        bool VerifyCaptcha(string name, string value);

        void Remove(string name);
        
    }
}