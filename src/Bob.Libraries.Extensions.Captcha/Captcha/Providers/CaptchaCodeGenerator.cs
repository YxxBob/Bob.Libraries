using System;
using System.Collections.Generic;
using System.Text;
using Bob.Libraries.Extensions.Captcha.Contracts;

namespace Bob.Libraries.Extensions.Captcha.Providers
{
    public class CaptchaCodeGenerator : ICaptchaCodeGenerator
    {
        public const string LowerLetter = "qwertyuiopasdfghjklzxcvbnm";
        public const string UpperLetter = "QWERTYUIOPASDFGHJKLZXCVBNM";
        public const string NumberStr = "0123456789";
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public string OutputText(bool hasLowerLetter, bool hasUpperLetter, bool hasNumber, int len)
        {
            string key = $"{hasLowerLetter}{hasUpperLetter}{hasNumber}{len}";
            if (!_dictionary.ContainsKey(key))
            {
                StringBuilder sb = new StringBuilder();
                if (hasNumber)
                {
                    sb.Append(NumberStr);
                }
                if (hasLowerLetter)
                {
                    sb.Append(LowerLetter);
                }
                if (hasUpperLetter)
                {
                    sb.Append(UpperLetter);
                }
                _dictionary.Add(key, sb.ToString());
            }

            var t = _dictionary[key].ToCharArray();
            string code = string.Empty;
            var rd = new Random();
            for (var i = 0; i < len; i++)
            {
                code += t[rd.Next(0, t.Length)];
            }
            return code;
        }
    }
}