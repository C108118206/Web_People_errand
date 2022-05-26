using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Web;

namespace AttendanceManagement.Models
{
    public class HttpResponse
    {
        public static HttpClient client = new HttpClient();
        public static HttpResponseMessage response = new HttpResponseMessage();
        public static readonly string Web_url = "http://60.249.202.153:8082"; //Web主機的URL
        public static readonly string url = "http://60.249.202.153:7000/api/"; //主機的URL
        public static readonly string CompanyGetCompanyHash = "Companies/Get_CompanyHash?"; //取得公司編號
        public static readonly string CompanyGetCompanyAddress = "Companies/Get_CompanyAddress?company_hash="; //取得公司地址
        public static readonly string CompanyEditCompanyAddress = "Companies/Update_CompanyAddress"; //(Put)修改公司地址
        public static readonly string CompanyLogin = "Companies/Login_Company?"; //公司登入
        public static readonly string CompanyGetTime = "Companies/Get_WorkTime_RestTime/"; //查看公司上下班時間
        public static readonly string CompanyEditTime = "Companies/Update_WorkTime_RestTime?"; //(PUT)編輯公司上下班時間
        public static readonly string CompanyEditManagerPassword = "Companies/UpdateCompanyPassword"; //(PUT)編輯公司密碼
        public static readonly string CompanyGetWorkRecord = "Companies/GetWorkRecord/"; //查看公司員工打卡紀錄api
        public static readonly string CompanyReviewEmployee = "Companies/Review_Employee/"; //查看公司待審核之員工帳號資料api
        public static readonly string CompanyPassEmployee = "Companies/Pass_Employee/";//查看公司審核通過之員工資料api
        public static readonly string CompanyDepartment = "EmployeeDepartmentTypes/";//查看公司部門資料api
        public static readonly string CompanyJobtitle = "EmployeeJobtitleTypes/";//查看公司職稱資料api
        public static readonly string CompanyLeaveType = "EmployeeLeaveTypes";//查看公司請假資料api
        public static readonly string CompanyReviewTripRecord = "Companies/Review_TripRecord/";//查看公司待審核公差資料api
        public static readonly string CompanyPassTripRecord = "Companies/Pass_TripRecord/";//查看公司已審核公差資料api
        public static readonly string CompanyGetTrip2Record = "Companies/Get_Trip2Record/";//查看公司公差資料2api
        public static readonly string CompanyDetailTrip2Record = "Companies/Detail_Trip2Record/";//查看詳細公差資料2api
        public static readonly string CompanyReviewLeaveRecord = "Companies/Review_LeaveRecord/";//查看公司待審核請假資料api
        public static readonly string CompanyPassLeaveRecord = "Companies/Pass_LeaveRecord/";//查看公司已審核請假資料api
        public static readonly string EmployeeSetInformation = "EmployeeInformations/set_information";//(PUT)賦予審核通過的員工部門及職稱API
        public static readonly string EmployeeRejectInformation = "EmployeeInformations/DeleteInformation/";//(Delete)拒絕審核員工帳號
        public static readonly string EmployeeEditInformation = "EmployeeInformations/edit_information";//(PUT)編輯員工資料
        public static readonly string EmployeeEnabled = "Employees/enabled_employee";//(PUT)停用或恢復員工帳號
        public static readonly string EmployeeReviewTripRecord = "EmployeeTripRecords/review_tripRecord";//(PUT)審核公差API
        public static readonly string EmployeeEditTripRecord = "EmployeeTripRecords/update_tripRecord";//(PUT)編輯公差API
        public static readonly string EmployeeReviewLeaveRecord = "EmployeeLeaveRecords/review_leaveRecord";//(PUT)審核請假API
        public static readonly string EmployeeEditLeaveRecord = "EmployeeLeaveRecords​/update_leaveRecord";//(PUT)編輯請假API
        public static readonly string CompanyAddDepartment = "EmployeeDepartmentTypes/add_department";//(POST)新增部門API
        public static readonly string CompanyEditDepartment = "EmployeeDepartmentTypes/UpdateDepartment";//(PUT)編輯部門名稱API
        public static readonly string CompanyDeleteDepartment = "EmployeeDepartmentTypes/DeleteDepartment/";//(Delete)刪除部門
        public static readonly string CompanyAddJobtitle = "EmployeeJobtitleTypes/add_jobtitle";//(POST)新增職稱API
        public static readonly string CompanyEditJobtitle = "EmployeeJobtitleTypes/UpdateJobtitle";//(PUT)編輯職稱API
        public static readonly string CompanyDeleteJobtitle = "EmployeeJobtitleTypes/DeleteJobtitle/";//(Delete)刪除職稱
        public static readonly string ManagerLogin = "Companies/Login_Manager?";//管理員登入
        public static readonly string ManagerGetCompanyHash = "Companies/Get_Manager?"; //管理員登入取得HASH跟NAME
        public static readonly string ManagerEditManagerPassword = "Companies/UpdateManagerPassword"; //(PUT)變更管理員密碼
        public static readonly string ManagerUpdateEnabled = "Companies/UpdateManagerEnabled"; //(PUT)啟用或停用管理員
        public static readonly string ManagerAdd = "Companies/AddManagerAccount"; //(PUT)新增管理員
        public static readonly string ManagerKey = "Employees/get_employee_manager_key/"; //取得ManagerKey
        public static readonly string ManagerKeyGetData = "Employees/get_employee_data/"; //用ManagerKey取得員工資料
        public static readonly string ManagerBool = "Companies/BoolManager/"; //判斷是否有此管理員帳號
        public static readonly string ManagerAll = "Companies/Get_All_Manager/"; //查看公司全部管理員
        public static readonly string EmployeeBoolRepeatEmail = "EmployeeInformations/BoolEmployeeInformationEmail?";//判斷EMAIL是否重複
        public static readonly string CompanyGetOrganization = "Companies/OrganizationChart/";//組織圖
        public static readonly string CompanyGetAllGeneralWorktime = "Companies/GetAllGeneralWorkTime/";//查看公司全部一般上下班設定
        public static readonly string CompanyGetAllFlexibleWorktime = "Companies/GetAllFlexibleWorkTime/";//查看公司全部彈性上下班設定
        public static readonly string CompanyEditGeneralWorktime = "EmployeeGeneralWorktimes/update_general_worktime";//修改公司一般上下班設定
        public static readonly string CompanyEditFlexibleWorktime = "EmployeeFlexibleWorktimes/update_flexible_worktime";//修改公司彈性上下班設定
        public static readonly string CompanyAddGeneralWorktime = "EmployeeGeneralWorktimes/add_general_worktime";//新增公司一般上下班設定
        public static readonly string CompanyAddFlexibleWorktime = "EmployeeFlexibleWorktimes/add_flexible_worktime";//新增公司彈性上下班設定
        public static readonly string CompanyDeleteFlexibleWorktime = "EmployeeFlexibleWorktimes/DeleteFlexibleWorktime/";//刪除公司彈性上下班設定
        public static readonly string CompanyDeleteGeneralWorktime = "EmployeeGeneralWorktimes/DeleteGeneralWorktime/";//刪除公司一般上下班設定
        public static readonly string CompanyGetEmployeeWorktime = "Companies/CompanyEmployeeWorkTime/";//查看公司所有部門職稱
        public static readonly string CompanyUpdateEmployeeWorktime = "Companies/UpdateEmployeeWorkTime";//編輯部門職稱上下班時間
        public static readonly string CompanyRenewEmployeeWorktime = "Companies/RenewWorktime";//編輯更新員工上下班時間
        public static readonly string CompanyGetManagerPermissions = "Companies/Get_Manager_Permissions?company_hash=";//取得權限設定
        public static readonly string CompanyAddManagerPermissions = "ManagerPermissions/add_manager_permissions";//新增權限設定
        public static readonly string CompanyUpdateManagerPermissions = "ManagerPermissions/update_manager_permissions";//編輯權限設定
        public static readonly string CompanyDeleteManagerPermissions = "ManagerPermissions/DeleteManagerPermissions/";//刪除權限設定
        public static readonly string CompanyGetManagerPermissionsCustomizations = "Companies/Get_Manager_Permissions_Customizations";//取得權限自訂設定
        public static readonly string CompanyAddManagerPermissionsCustomizations = "ManagerPermissionsCustomizations/add_manager_permissions_customization";//新增權限自訂設定
        public static readonly string CompanyDeleteManagerPermissionsCustomizations = "ManagerPermissionsCustomizations/DeleteManagerPermissionsCustomization/";//刪除權限自訂設定
        public static readonly string CompanyGetManagerAccountPermissions = "Companies/CompanyManagerAccountPermissions/"; //取得角色套用權限資料
        public static readonly string CompanyUpdateManagerAccountPermissions = "Companies/UpdateManagerAccountPermissions"; //編輯角色套用權限資料
        public static readonly string CompanyGetManagerRolePermissions = "Companies/GetManagerRolePermissions/"; //取得目前登入的管理員的權限設定
        public static readonly string ManagerGetReviewLeaveRecord = "EmployeeLeaveRecords/Review_LeaveRecord?hash_company="; //管理員取得待審核請假資料
        public static readonly string ManagerGetPassLeaveRecord2 = "Companies/Pass_LeaveRecord2/"; //管理員取得直屬員工已審核請假資料
        public static readonly string ManagerGetPassLeaveRecord3 = "Companies/Pass_LeaveRecord3/"; //管理員取得自訂對象已審核請假資料
        public static readonly string ManagerGetWorkRecord2 = "Companies/GetWorkRecord2/"; //管理員取得直屬員工打卡資料
        public static readonly string ManagerGetWorkRecord3 = "Companies/GetWorkRecord3/"; //管理員取得自訂對象打卡資料
        public static readonly string ManagerGetPassEmployee2 = "Companies/Manager_GetPassEmployee2/"; //管理員取得直屬員工資料
        public static readonly string ManagerGetPassEmployee3 = "Companies/Manager_GetPassEmployee3/"; //管理員取得自訂對象資料
        public static readonly string ManagerGetTrip2Record2 = "Companies/Get_Trip2Record2/"; //管理員取得直屬員工公差資料
        public static readonly string ManagerGetTrip2Record3 = "Companies/Get_Trip2Record3/"; //管理員取得自訂對象公差資料
        public static readonly string ManagerUpdateAgent = "Companies/UpdateManagerAgent"; //編輯職務代理人
        public static readonly string ManageGetBossPermissions = "Companies/GetbossSettingPermissions/"; //職務代理人取得代理對象的設定權限
        public static readonly string ManageBoolAgent = "EmployeeLeaveRecords/BoolAgentReviewLeave/"; //判斷是否可以執行代理權限
        public static readonly string ManagerForegetPassword = "Companies/Forget_Manager?code="; //找到管理員HASH
        public static readonly string AddLog = "Companies/AddLog"; //新增LOG
        public static readonly string CompanyGetPositionDifference = "Companies/GetCompanyPositionDifference/"; //打卡誤差值
        public static readonly string CompanyGetSettingTrip2Enabled = "Companies/GetCompanySettingTrip2Enabled/"; //是否開啟到站
        public static readonly string CompanyGetSettingWorkRecordEnabled = "Companies/GetCompanySettingWorkRecordEnabled/"; //是否開啟定位打卡
        public static readonly string CompanyUpdatePositionDifference = "Companies/UpdateCompanyPositionDifference"; //更新公司打卡誤差值
        public static readonly string CompanyUpdateSettingTrip2Enabled = "Companies/UpdateCompanySettingTrip2Enabled"; //更新公司是否開啟到站
        public static readonly string CompanyUpdateSettingWorkRecordEnabled = "Companies/UpdateCompanySettingWorkRecordEnabled"; //更新是否開啟定位打卡


        //public static readonly string sendGmail = "https://localhost:44311/api/Companies/sendGmailAsync?to_email="; //寄EMAIL

        public static async System.Threading.Tasks.Task sendGmailAsync(string to_email,string email_subject, string email_body)//寄EMAIL
        {
            ////連上WebAPI
            //response = await client.GetAsync(sendGmail + to_email + "&email_subject=" + email_subject + "&email_body=" + email_body);
            ////取得API回傳的打卡紀錄內容
            //GetResponse = await response.Content.ReadAsStringAsync();
            ////解析打卡紀錄之JSON內容
            //bool result = JsonConvert.DeserializeObject<bool>(GetResponse);

            //return result;
            try
            {
                MailMessage mail = new MailMessage();
                //前面是發信email後面是顯示的名稱
                mail.From = new MailAddress("like3yy@gmail.com", "差勤打卡");

                //收信者email
                mail.To.Add(to_email);

                //設定優先權
                mail.Priority = MailPriority.Normal;

                //標題
                mail.Subject = email_subject;

                //內容
                mail.Body = email_body;

                //內容使用html
                mail.IsBodyHtml = true;

                //設定gmail的smtp (這是google的)
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

                //您在gmail的帳號密碼
                MySmtp.Credentials = new System.Net.NetworkCredential("like3yy@gmail.com", "ekwxscpgiylihnvk");

                //開啟ssl
                MySmtp.EnableSsl = true;

                //發送郵件
                await MySmtp.SendMailAsync(mail);

                //放掉宣告出來的MySmtp
                MySmtp = null;

                //放掉宣告出來的mail
                mail.Dispose();
                Console.WriteLine("成功發送EMAIL通知!");
            }
            catch (Exception) 
            {
                Console.WriteLine("發送EMAIL通知失敗!");
            }
        }
        public static string GetResponse { get; set; } //存取API回傳的內容

    }
}