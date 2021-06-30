using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttendanceManagement.Models;

namespace AttendanceManagement.Controllers
{
    public class PunchRecordsController : Controller
    {
        // GET: PunchRecords
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {

            //輸入公司代碼取得打卡紀錄
            List<StaffModel> work_record = await StaffModel.Get_WorkRecordAsync("ACF02E147E1AC0E66CEE91D9DD8CA4");
            int[] Num = new int[work_record.Count];//編號
            string[] Name = new string[work_record.Count];//員工名稱
            DateTime[] WorkTime = new DateTime[work_record.Count];//上班時間
            DateTime[] RestTime = new DateTime[work_record.Count];//下班時間

            for (int i = 0; i < work_record.Count; i++)//將打卡紀錄分別存入欄位
            {
                Num[i] = work_record[i].Num;
                Name[i] = work_record[i].Name;
                WorkTime[i] = work_record[i].WorkTime;
                RestTime[i] = work_record[i].RestTime;

            }

            ViewBag.num = Num;//編號
            ViewBag.name = Name;//員工名稱
            ViewBag.worktime = WorkTime;//上班時間
            ViewBag.resttime = RestTime;//下班時間

            return View();
        }
    }
}