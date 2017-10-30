using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Bob.Demo.App.Models;
using Bob.Libraries.Extensions.Captcha;
using Bob.Libraries.Extensions.QRCode;
using Bob.Libraries.Extensions.WeChat.Attributes;
using Bob.Libraries.Extensions.WeChat.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Bob.Demo.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ValidateCaptcha]
        public IActionResult TestCaptcha(bool captchaValid)
        {
            return Content(captchaValid.ToString());
        }

        public async Task<IActionResult> WeChat(string returnUrl)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "test")
                        };
          
            var claimsIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);
            return Redirect(returnUrl);
        }

        [WeChatAutoLogin]
        public IActionResult AutoLogin()
        {
            return Content("登录成功");
        }

        public IActionResult ShowOpenId()
        {
            return Content(HttpContext.Session.GetOAuthAccessToken().openid);
        }

        public IActionResult QRCode()
        {
            return new FileContentResult(QRCoderHelper.RenderQrCode("www.baidu.com").ToBytes(),"image/png");
        }
    }
}
