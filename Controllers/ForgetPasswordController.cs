using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttendanceManagement.Models;

namespace AttendanceManagement.Controllers
{
    public class ForgetPasswordController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Submit(string code, string email)
        {

            string managerhash = await CompanyManagerModel.ForgetPassword_GetManagerHash(code, email);
            if (managerhash!="")
            {
                string managerkey = await CompanyManagerModel.GetManagerKey(managerhash);

                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員忘記密碼", $"<h1>您好!請問您忘記密碼了嗎?</h1><p>請於三天內至以下網址重新設定您的管理員密碼，逾期則設定失敗。</p><p><a href='{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}'>{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}</a></p>");
                return Content($"<script>alert('已發送設定密碼之郵件至您的信箱，請至信箱內查看。');window.location='/Account/index';</script>");
            }
            else
                return Content($"<script>alert('此Email並未註冊管理員，請確認資料是否填寫錯誤，若忘記Email可至差勤打卡APP之個人資料查看。');history.go(-1);</script>");
        }
    }
}