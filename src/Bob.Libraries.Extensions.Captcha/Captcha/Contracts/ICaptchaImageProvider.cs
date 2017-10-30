namespace Bob.Libraries.Extensions.Captcha.Contracts
{
    /// <summary>
    /// Captcha Image Provider
    /// </summary>
    public interface ICaptchaImageProvider
    {
        /// <summary>
        /// Creates the captcha image.
        /// </summary>
        byte[] DrawCaptcha(string message, float emSize, int imageWidth, int imageHeight);
    }
}
