using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttendanceManagement.Models;
using Newtonsoft.Json;


namespace AttendanceManagement.Controllers
{
    public class EmployeeDataController : Controller
    {
        // GET: EmployeeData
        public async Task<ActionResult> Index()//員工管理初始頁面
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }
            //輸入公司取得全部的管理員
            List<Manager> managers = await CompanyManagerModel.GetAllManager(Session["company_hash"].ToString());
            //輸入公司代碼取得待審核資料
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(Session["company_hash"].ToString());
            //輸入公司代碼取得已審核資料(會變動)
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());
            //輸入公司代碼取得部門資料
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(Session["company_hash"].ToString());
            //輸入公司代碼取得職稱資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(Session["company_hash"].ToString());
            //輸入公司代碼取得全部的已審核資料(不會變動)
            List<PassEmployee> all_passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());
            //取得權限
            List<ManagerPermissions> managerPermissions = await CompanyManagerPermissionsModel.Get_ManagerPermissions(Session["company_hash"].ToString());


            if (Session["hash_account"] != null)
            {
                int index = managers.FindIndex(item => item.ManagerHash.Equals(Session["hash_account"].ToString()));

                if (managers[index].PermissionsId == null) { }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 1) { }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 2)
                {
                    all_passEmployees = await PassEmployeeModel.ManagerPassEmployees2(Session["hash_account"].ToString());
                    passEmployees = await PassEmployeeModel.ManagerPassEmployees2(Session["hash_account"].ToString());
                }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 3)
                {
                    all_passEmployees = await PassEmployeeModel.ManagerPassEmployees3(Session["hash_account"].ToString());
                    passEmployees = await PassEmployeeModel.ManagerPassEmployees3(Session["hash_account"].ToString());
                }
                else 
                {
                    if (await CompanyManagerPermissionsModel.Manager_Bool_Agent(Session["hash_account"].ToString()))
                    {
                        int bossindex = managers.FindIndex(item => item.AgentHash.Equals(Session["hash_account"].ToString()));

                        if (managers[bossindex].PermissionsId == null) { }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 1) { }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 2)
                        {
                            all_passEmployees = await PassEmployeeModel.ManagerPassEmployees3(Session["hash_account"].ToString());
                            passEmployees = await PassEmployeeModel.ManagerPassEmployees2(managers[bossindex].ManagerHash);
                        }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 3)
                        {
                            all_passEmployees = await PassEmployeeModel.ManagerPassEmployees3(Session["hash_account"].ToString());
                            passEmployees = await PassEmployeeModel.ManagerPassEmployees3(managers[bossindex].ManagerHash);
                        }
                        else 
                        {
                            reviewEmployees = await ReviewEmployeeModel.ReviewEmployees("n");
                            all_passEmployees = await PassEmployeeModel.PassEmployees("n");
                            passEmployees = await PassEmployeeModel.PassEmployees("n");
                        }
                    }
                    else 
                    {
                        reviewEmployees = await ReviewEmployeeModel.ReviewEmployees("n");
                        all_passEmployees = await PassEmployeeModel.PassEmployees("n");
                        passEmployees = await PassEmployeeModel.PassEmployees("n");
                    }
                    
                }
            }

            ViewBag.managers = managers;//全部的管理員
            ViewBag.all_passemployee = all_passEmployees;//全部的已審核資料
            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料   
            ViewBag.pass_employee = passEmployees; //審核過資料
            return View();
        }

        public async Task<ActionResult> Edit(string id)//員工管理修改頁面
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料
            int num = passEmployees.FindIndex(item => item.HashAccount.Equals(id));//員工索引值

            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(Session["company_hash"].ToString());//輸入公司代碼取得部門資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(Session["company_hash"].ToString());//輸入公司代碼取得職稱資料

            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.pass_employee = passEmployees;//待審核資料
            ViewBag.Num = num;//員工索引值  
            return View();
        }
   
        public async Task<ActionResult> Check(string id)//員工管理審核頁面
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(Session["company_hash"].ToString());//待審核資料
            int num = reviewEmployees.FindIndex(item => item.HashAccount.Equals(id));//員工索引值
            
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(Session["company_hash"].ToString());//輸入公司代碼取得部門資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(Session["company_hash"].ToString());//輸入公司代碼取得職稱資料

            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料
            ViewBag.Num = num;//員工索引值   
            return View("Review");
        }

        [HttpPost]//員工編輯頁面，提升管理員按鈕
        public async Task<string> AddManager(string email, string id)
        {
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料

            string managerkey = await CompanyManagerModel.GetManagerKey(id);
            if (Session["hash_account"] != null)
            {
                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;
                
                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡提升後台管理員通知", $"<h1>您好!您已被邀請成為差勤打卡後台管理員</h1><p>請於三天內至以下網址設定您的管理員密碼，逾期則提升管理員失敗。</p><p><a href='{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}'>{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}</a></p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
            }
            else
            {
                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡提升後台管理員通知", $"<h1>您好!您已被邀請成為差勤打卡後台管理員</h1><p>請於三天內至以下網址設定您的管理員密碼，逾期則提升管理員失敗。</p><p><a href='{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}'>{Models.HttpResponse.Web_url}/PasswordSetting/index?key={managerkey}</a></p>");
            }

            string result = "已發送提升管理員請求至員工信箱！若員工三天內未回覆提升請求，則提升管理員失敗，請重新邀請成為管理員。";
            return result;

        }

        [HttpPost]//員工管理審核頁面，審核按鈕
        public async Task<ActionResult> SetEmployeeInformation( bool?manager,string managerhash,string email,string id,string Button, int department, int jobtitle)
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }

            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(Session["company_hash"].ToString());//待審核資料

            bool result = false;
            string str_result = "";
            if (Button.Equals("SaveButton"))
            {
                result = await SetEmployeeModel.SetEmployees(id, managerhash, department, jobtitle);//PUT部門及職稱

                if (result)
                {
                    if (Session["hash_account"] != null)
                    {
                        string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                        string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號審核通知", $"<h1>差勤打卡帳號審核成功</h1><p>請至差勤打卡APP重新進行登入，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                    }
                    else 
                    {
                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號審核通知", "<h1>差勤打卡帳號審核成功</h1><p>請至差勤打卡APP重新進行登入，如有問題請連繫相關主管。</p>");
                    }

                    if (manager == true)
                    {
                        str_result = await AddManager(email, id);
                        return Content($"<script>alert('審核完成！{str_result}');window.location='/EmployeeData/index';</script>");

                    }
                    return RedirectToAction("index");
                }
                else
                    return Content("<script>alert('審核失敗！如有問題請連繫後台');history.go(-1);</script>");
            }
            else 
            {
                result = await SetEmployeeModel.RejectEmployees(id);//Delete員工

                if (result)
                {
                    if (Session["hash_account"] != null)
                    {
                        string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                        string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號審核通知", $"<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>審核失敗</h1><p>如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                    }
                    else
                    {
                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號審核通知", "<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>審核失敗</h1><p>如有問題請連繫相關主管。</p>");
                    }
                    return RedirectToAction("index");
                }
                else
                    return Content("<script>alert('審核失敗！如有問題請連繫後台');history.go(-1);</script>");
            }
        }
        [HttpPost]//員工管理修改頁面，修改按鈕
        public async Task<ActionResult> EditEmployee(string agent, string managerhash, bool? old_enabled, string Button, string id, string name, string phone, string email,string old_email, int department, int jobtitle,bool? manager,bool? disable)
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }
            bool? ManagerStatus = null;
            //輸入公司取得全部的管理員
            List<Manager> managers = await CompanyManagerModel.GetAllManager(Session["company_hash"].ToString());
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料
            foreach (var m in managers)
            {
                if (m.ManagerHash.Equals(id)) 
                {
                    ManagerStatus = m.Enabled;
                }
            }
            string result = "";
            bool update_result = false;
            bool Enabled_result = false;
            bool Manager_Enabled_result = false;
            int isManager = managers.FindIndex(item => item.ManagerHash.Equals(id));

            if (Button.Equals("RenewButton"))
            {
                if (!email.Equals(old_email))
                {
                    bool repeat = await PassEmployeeModel.EmployeeBoolEmail(Session["company_hash"].ToString(), email);
                    if (repeat)
                    {
                        return Content($"<script>alert('此Email已被註冊，請使用別的信箱！');window.location='/EmployeeData/index';</script>");
                    }
                    else
                    {
                        if (isManager != -1)
                        {
                            bool Agent_result = await CompanyManagerModel.UpdateManagerAgent(id, agent);//(PUT)更新職務代理人
                            if (Agent_result == false)
                            {
                                return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                            }

                        }
                        if (disable == true && old_enabled == true)//如果停用帳號打勾且原始帳號狀態為使用中
                        {
                            Enabled_result = await PassEmployeeModel.EnabledEmployees(id, false);//(PUT)更新員工資料為停用
                            if (Enabled_result)
                            {
                                if (Session["hash_account"] != null)
                                {
                                    string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                    string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", $"<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                                }
                                else
                                {
                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p>");
                                }
                            }
                            else
                                return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                        }
                        else if (disable != true && old_enabled == false)//如果停用帳號沒打勾且原始狀態為停用
                        {
                            Enabled_result = await PassEmployeeModel.EnabledEmployees(id, true);//(PUT)更新員工資料為使用中
                            if (Enabled_result)
                            {
                                if (Session["hash_account"] != null)
                                {
                                    string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                    string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", $"<h1>您的差勤打卡帳號已恢復使用權限</h1><p>請至差勤打卡APP確認，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                                }
                                else
                                {
                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號已恢復使用權限</h1><p>請至差勤打卡APP確認，如有問題請連繫相關主管。</p>");
                                }
                            }
                            else
                                return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                        }

                        if (manager == true && ManagerStatus == false)//如果提升管理員打勾且管理員狀態為停用
                        {
                            Manager_Enabled_result = await CompanyManagerModel.UpdateManagerEnabled(id, true);//(PUT)更新管理員狀態為啟用
                            if (Manager_Enabled_result)
                            {
                                result += "已恢復此帳號之管理員權限。";
                                if (Session["hash_account"] != null)
                                {
                                    string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                    string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", $"<h1>您的差勤打卡管理員帳號已恢復使用權限</h1><p>請至差勤打卡後台確認，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                                }
                                else
                                {
                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", "<h1>您的差勤打卡管理員帳號已恢復使用權限</h1><p>請至差勤打卡後台確認，如有問題請連繫相關主管。</p>");
                                }
                            }
                            else
                                return Content("<script>alert('管理員狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                        }
                        else if (manager == true && ManagerStatus == null)//如果提升管理員打勾且不是管理員
                        {
                            result += await AddManager(email, id);//邀請成為管理員
                        }
                        else if (manager != true && ManagerStatus == true)//如果取消勾選管理員且管理員狀態是使用中
                        {
                            Manager_Enabled_result = await CompanyManagerModel.UpdateManagerEnabled(id, false);//(PUT)更新管理員狀態為停用

                            if (Manager_Enabled_result)
                            {
                                result += "已停用此帳號之管理員權限。";
                                if (Session["hash_account"] != null)
                                {
                                    string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                    string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", $"<h1>您的差勤打卡管理員帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                                }
                                else
                                {
                                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", "<h1>您的差勤打卡管理員帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p>");
                                }
                            }
                            else
                                return Content("<script>alert('管理員狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                        }
                        update_result = await PassEmployeeModel.RenewEmployees(id, managerhash, name, phone, email, department, jobtitle);//(PUT)更新員工資料
                    }
                }
                else
                {
                    if (isManager != -1)
                    {
                        bool Agent_result = await CompanyManagerModel.UpdateManagerAgent(id, agent);//(PUT)更新職務代理人
                        if (Agent_result == false)
                        {
                            return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                        }

                    }
                    if (disable == true && old_enabled == true)//如果停用帳號打勾且原始帳號狀態為使用中
                    {
                        Enabled_result = await PassEmployeeModel.EnabledEmployees(id, false);//(PUT)更新員工資料為停用
                        if (Enabled_result)
                        {
                            if (Session["hash_account"] != null)
                            {
                                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", $"<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                            }
                            else
                            {
                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p>");
                            }
                        }
                        else
                            return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                    }
                    else if (disable != true && old_enabled == false)//如果停用帳號沒打勾且原始狀態為停用
                    {
                        Enabled_result = await PassEmployeeModel.EnabledEmployees(id, true);//(PUT)更新員工資料為使用中
                        if (Enabled_result)
                        {
                            if (Session["hash_account"] != null)
                            {
                                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", $"<h1>您的差勤打卡帳號已恢復使用權限</h1><p>請至差勤打卡APP確認，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                            }
                            else
                            {
                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號已恢復使用權限</h1><p>請至差勤打卡APP確認，如有問題請連繫相關主管。</p>");
                            }
                        }
                        else
                            return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                    }

                    if (manager == true && ManagerStatus == false)//如果提升管理員打勾且管理員狀態為停用
                    {
                        Manager_Enabled_result = await CompanyManagerModel.UpdateManagerEnabled(id, true);//(PUT)更新管理員狀態為啟用
                        if (Manager_Enabled_result)
                        {
                            result += "已恢復此帳號之管理員權限。";
                            if (Session["hash_account"] != null)
                            {
                                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知",$"<h1>您的差勤打卡管理員帳號已恢復使用權限</h1><p>請至差勤打卡後台確認，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                            }
                            else
                            {
                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", "<h1>您的差勤打卡管理員帳號已恢復使用權限</h1><p>請至差勤打卡後台確認，如有問題請連繫相關主管。</p>");
                            }
                        }
                        else
                            return Content("<script>alert('管理員狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                    }
                    else if (manager == true && ManagerStatus == null)//如果提升管理員打勾且不是管理員
                    {
                        result += await AddManager(email, id);//邀請成為管理員
                    }
                    else if (manager != true && ManagerStatus == true)//如果取消勾選管理員且管理員狀態是使用中
                    {
                        Manager_Enabled_result = await CompanyManagerModel.UpdateManagerEnabled(id, false);//(PUT)更新管理員狀態為停用

                        if (Manager_Enabled_result)
                        {
                            result += "已停用此帳號之管理員權限。";
                            if (Session["hash_account"] != null)
                            {
                                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", $"<h1>您的差勤打卡管理員帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                            }
                            else
                            {
                                await Models.HttpResponse.sendGmailAsync(email, "差勤打卡管理員帳號狀態更新通知", "<h1>您的差勤打卡管理員帳號</h1><h1 style='color:red;'>已遭停用</h1><p>如有問題請連繫相關主管。</p>");
                            }
                        }
                        else
                            return Content("<script>alert('管理員狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
                    }
                    update_result = await PassEmployeeModel.RenewEmployees(id, managerhash, name, phone, email, department, jobtitle);//(PUT)更新員工資料
                }
                if (update_result)
                {
                    if (Session["hash_account"] != null)
                    {
                        string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                        string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡資料更新通知", $"<h1>您的差勤打卡個人資料已更新</h1><p>請至差勤打卡APP個人資料確認更新內容，如有問題請連繫相關主管。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                    }
                    else
                    {
                        await Models.HttpResponse.sendGmailAsync(email, "差勤打卡資料更新通知", "<h1>您的差勤打卡個人資料已更新</h1><p>請至差勤打卡APP個人資料確認更新內容，如有問題請連繫相關主管。</p>");
                    }
                    return Content($"<script>alert('更新成功！{result}');window.location='/EmployeeData/index';</script>");
                }
                else
                    return Content($"<script>alert('更新失敗！如有問題請連繫後台');history.go(-1);</script>");

                
            }
            return RedirectToAction("index");
        }

        //[HttpPost]//員工管理修改頁面，停用或恢復按鈕
        //public async Task<ActionResult> EnabledEmployee(int num,string id, bool Enabled)
        //{
        //    List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料

        //    bool result = false;
        //    result = await PassEmployeeModel.EnabledEmployees(id,(bool)Enabled);//(PUT)更新員工資料

        //    if (result)
        //    {
        //        if (Enabled)
        //            AttendanceManagement.Models.HttpResponse.sendGmail(passEmployees[num].Email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號已恢復使用權限</h1><p>請至差勤打卡APP確認，如有問題請連繫後台。</p>");

        //        else
        //            AttendanceManagement.Models.HttpResponse.sendGmail(passEmployees[num].Email, "差勤打卡帳號狀態更新通知", "<h1>您的差勤打卡帳號已遭停用</h1><p>如有問題請連繫後台。</p>");
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return Content("<script>alert('狀態更新失敗！如有問題請連繫後台');history.go(-1);</script>");
        //}
        [HttpPost]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());//已審核資料

            string email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(id))].Email;
            bool result = await SetEmployeeModel.RejectEmployees(id);//Delete員工
            
            if (Session["hash_account"] != null)
            {
                string manager_email = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Email;
                string manager_name = passEmployees[passEmployees.FindIndex(item => item.HashAccount.Equals(Session["hash_account"]))].Name;

                if (result)
                {
                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號刪除通知", $"<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭刪除</h1><p>如有問題請與主管聯繫。</p><p>審核人：{manager_name}</p><p>聯絡信箱：{manager_email}</p>");
                    return RedirectToAction("index");
                }
                else
                    return Content("<script>alert('審核失敗！如有問題請連繫後台');history.go(-1);</script>");
            }
            else 
            {
                if (result)
                {
                    await Models.HttpResponse.sendGmailAsync(email, "差勤打卡帳號刪除通知", $"<h1>您的差勤打卡帳號</h1><h1 style='color:red;'>已遭刪除</h1><p>如有問題請與相關主管聯繫。</p>");
                    return RedirectToAction("index");
                }
                else
                    return Content("<script>alert('審核失敗！如有問題請連繫後台');history.go(-1);</script>");
            }
        }
        [HttpGet]//已審核員工資料篩選
        public async Task<ActionResult> SearchEmployee(string department, string jobtitle, string employee_name)
        {
            if (Session["company_hash"] == null)
            {
                return RedirectToAction("Index", "Account", null);
            }
            //輸入公司取得全部的管理員
            List<Manager> managers = await CompanyManagerModel.GetAllManager(Session["company_hash"].ToString());
            //輸入公司代碼取得待審核資料
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(Session["company_hash"].ToString());
            //輸入公司代碼取得全部的已審核資料
            List<PassEmployee> all_passEmployees = await PassEmployeeModel.PassEmployees(Session["company_hash"].ToString());
            //輸入公司代碼取得部門資料
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(Session["company_hash"].ToString());
            //輸入公司代碼取得職稱資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(Session["company_hash"].ToString());
            //取得權限
            List<ManagerPermissions> managerPermissions = await CompanyManagerPermissionsModel.Get_ManagerPermissions(Session["company_hash"].ToString());


            if (Session["hash_account"] != null)
            {
                int index = managers.FindIndex(item => item.ManagerHash.Equals(Session["hash_account"].ToString()));

                if (managers[index].PermissionsId == null) { }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 1) { }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 2)
                {
                    all_passEmployees = await PassEmployeeModel.ManagerPassEmployees2(Session["hash_account"].ToString());
                }
                else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[index].PermissionsId)].EmployeeDisplay == 3)
                {
                    all_passEmployees = await PassEmployeeModel.ManagerPassEmployees3(Session["hash_account"].ToString());
                }
                else
                {
                    if (await CompanyManagerPermissionsModel.Manager_Bool_Agent(Session["hash_account"].ToString()))
                    {
                        int bossindex = managers.FindIndex(item => item.AgentHash.Equals(Session["hash_account"].ToString()));

                        if (managers[bossindex].PermissionsId == null) { }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 1) { }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 2)
                        {
                            all_passEmployees = await PassEmployeeModel.ManagerPassEmployees2(managers[bossindex].ManagerHash);
                        }
                        else if (managerPermissions[managerPermissions.FindIndex(item => item.PermissionsId == managers[bossindex].PermissionsId)].EmployeeDisplay == 3)
                        {
                            all_passEmployees = await PassEmployeeModel.ManagerPassEmployees3(managers[bossindex].ManagerHash);
                        }
                        else
                        {
                            reviewEmployees = await ReviewEmployeeModel.ReviewEmployees("n");
                            all_passEmployees = await PassEmployeeModel.PassEmployees("n");
                        }
                    }
                    else
                    {
                        reviewEmployees = await ReviewEmployeeModel.ReviewEmployees("n");
                        all_passEmployees = await PassEmployeeModel.PassEmployees("n");
                    }

                }
            }
            //放入篩選後的已審核資料
            List<PassEmployee> searchEmployees = new List<PassEmployee>();

            int departmentIndex = departments.FindIndex(item => item.Name.Equals(department));//部門索引值
            int jobtitleIndex = jobtitles.FindIndex(item => item.Name.Equals(jobtitle));//職稱索引值
            int num = all_passEmployees.FindIndex(item => item.HashAccount.Equals(employee_name));//員工索引值

            if (departmentIndex == -1 && jobtitleIndex == -1 && num == -1)//沒有輸入篩選條件就按搜尋，顯示全部資料
                return RedirectToAction("index");
            else if(departmentIndex != -1 && jobtitleIndex != -1 && num != -1)//三個篩選條件都輸入
                searchEmployees = await PassEmployeeModel.SearchEmployees3(all_passEmployees, Session["company_hash"].ToString(), department, jobtitle, employee_name);
            else if ((departmentIndex != -1 && jobtitleIndex != -1) || (departmentIndex!=-1 && num != -1) || (jobtitleIndex!=-1 && num!= -1))//只輸入兩個篩選條件
                searchEmployees = await PassEmployeeModel.SearchEmployees2(all_passEmployees, Session["company_hash"].ToString(), department, jobtitle, employee_name);
            else searchEmployees = await PassEmployeeModel.SearchEmployees1(all_passEmployees, Session["company_hash"].ToString(), department, jobtitle, employee_name);//只輸入一個篩選條件

            ViewBag.managers = managers;//全部的管理員
            ViewBag.all_passemployee = all_passEmployees;//全部的已審核資料
            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料   
            ViewBag.pass_employee = searchEmployees; //篩選後的審核過資料
            return View("Index");
        }
    }
}