using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _114_1_database_final_project.Controllers
{
    public class AccountController : Controller
    {
        // 1. 顯示登入頁面
        [HttpGet]
        public IActionResult Login()
        {
            // 如果已經登入過，直接導向首頁
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // 2. 處理登入動作 (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // 硬編碼驗證：帳號 admin，密碼 admin
            if (username == "admin" && password == "admin")
            {
                // 建立使用者身分 (Claims)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    // 這裡設定 Cookie 保持登入的時間 (例如 30 分鐘)
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                // 執行登入 (寫入 Cookie)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // 登入成功，導回首頁
                return RedirectToAction("Index", "Home");
            }

            // 登入失敗
            ViewBag.ErrorMessage = "帳號或密碼錯誤";
            return View();
        }

        // 3. 登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}