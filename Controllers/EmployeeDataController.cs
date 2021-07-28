﻿using System;
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
        private string company_hash = Models.HttpResponse.CompanyHash;
        private string url = Models.HttpResponse.url;
        private string EmployeeSetInformation = Models.HttpResponse.EmployeeSetInformation;
        // GET: EmployeeData
        public async Task<ActionResult> Index()//員工管理初始頁面
        {
            //輸入公司代碼取得待審核資料
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(company_hash);
            //輸入公司代碼取得已審核資料(會變動)
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(company_hash);
            //輸入公司代碼取得部門資料
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(company_hash);
            //輸入公司代碼取得職稱資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(company_hash);
            //輸入公司代碼取得全部的已審核資料(不會變動)
            List<PassEmployee> all_passEmployees = await PassEmployeeModel.PassEmployees(company_hash);

            ViewBag.all_passemployee = all_passEmployees;//全部的已審核資料
            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料   
            ViewBag.pass_employee = passEmployees; //審核過資料
            return View();
        }

        public async Task<ActionResult> Edit(string id)//員工管理修改頁面
        {
            List<PassEmployee> passEmployees = await PassEmployeeModel.PassEmployees(company_hash);//已審核資料
            int num = passEmployees.FindIndex(item => item.HashAccount.Equals(id));//員工索引值

            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(company_hash);//輸入公司代碼取得部門資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(company_hash);//輸入公司代碼取得職稱資料

            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.pass_employee = passEmployees;//待審核資料
            ViewBag.Num = num;//員工索引值  
            return View();
        }
   
        public async Task<ActionResult> Check(string id)//員工管理審核頁面
        {
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(company_hash);//待審核資料
            int num = reviewEmployees.FindIndex(item => item.HashAccount.Equals(id));//員工索引值
            
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(company_hash);//輸入公司代碼取得部門資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(company_hash);//輸入公司代碼取得職稱資料

            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料
            ViewBag.Num = num;//員工索引值   
            return View("Review");
        }

        [HttpPost]//員工管理審核頁面，審核按鈕
        public async Task<ActionResult> SetEmployeeInformation(string Button, string phonecode, string department, string jobtitle)
        {
            if (Button.Equals("SaveButton"))
            {
                List<Department> departments = await DepartmentModel.Get_DepartmentAsync(company_hash);//輸入公司代碼取得部門資料
                List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(company_hash);//輸入公司代碼取得職稱資料
                int departmentIndex = departments.FindIndex(item => item.Name.Equals(department));//部門索引值
                int jobtitleIndex = jobtitles.FindIndex(item => item.Name.Equals(jobtitle));//職稱索引值

                List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(company_hash);//待審核資料
                int num = reviewEmployees.FindIndex(item => item.PhoneCode.Equals(phonecode));//員工索引值

                string hashaccount = reviewEmployees[num].HashAccount;

                if (departmentIndex == -1 || jobtitleIndex == -1)
                {
                    return Content("<script>alert('審核失敗，請確認是否有賦予職稱及部門！');history.go(-1);</script>");
                }

                bool result = await SetEmployeeModel.SetEmployees(hashaccount, departments[departmentIndex].DepartmentID, jobtitles[jobtitleIndex].JobTitleID);//PUT部門及職稱

                if (result)
                {
                    SetEmployeeModel.sendGmail(reviewEmployees[num].Email, "成功");
                    return Redirect("/EmployeeData/Index");
                }
                else
                    return Content("<script>alert('審核失敗！如有問題請連繫後台');history.go(-1);</script>");
            }
            return Redirect("/EmployeeData/Index");
        }
        [HttpPost]//員工管理修改頁面，修改按鈕
        public async Task<ActionResult> EditEmployee(string Button, string phonecode, string department, string jobtitle)
        {
            if(Button.Equals("RenewButton"))
                return Content("<script>alert('目前不能用ㄏㄏ');history.go(-1);</script>");
            return Redirect("/EmployeeData/Index");
        }
        [HttpGet]
        public async Task<ActionResult> SearchEmployee(string department, string jobtitle, string employee_name)//搜尋
        {
            //輸入公司代碼取得待審核資料
            List<ReviewEmployee> reviewEmployees = await ReviewEmployeeModel.ReviewEmployees(company_hash);
            //輸入公司代碼取得全部的已審核資料
            List<PassEmployee> all_passEmployees = await PassEmployeeModel.PassEmployees(company_hash);
            //輸入公司代碼取得部門資料
            List<Department> departments = await DepartmentModel.Get_DepartmentAsync(company_hash);
            //輸入公司代碼取得職稱資料
            List<JobTitle> jobtitles = await JobtitleModel.Get_JobtitleAsync(company_hash);

            //放入篩選後的已審核資料
            List<PassEmployee> searchEmployees = new List<PassEmployee>();

            int departmentIndex = departments.FindIndex(item => item.Name.Equals(department));//部門索引值
            int jobtitleIndex = jobtitles.FindIndex(item => item.Name.Equals(jobtitle));//職稱索引值
            int num = all_passEmployees.FindIndex(item => item.Name.Equals(employee_name));//員工索引值

            if (departmentIndex == -1 && jobtitleIndex == -1 && num == -1)//沒有輸入篩選條件就按搜尋，顯示全部資料
                return Redirect("/EmployeeData/Index");
            else if(departmentIndex != -1 && jobtitleIndex != -1 && num != -1)//三個篩選條件都輸入
                searchEmployees = await PassEmployeeModel.SearchEmployees3(company_hash, department, jobtitle, employee_name);
            else if ((departmentIndex != -1 && jobtitleIndex != -1) || (departmentIndex!=-1 && num != -1) || (jobtitleIndex!=-1 && num!= -1))//只輸入兩個篩選條件
                searchEmployees = await PassEmployeeModel.SearchEmployees2(company_hash, department, jobtitle, employee_name);
            else searchEmployees = await PassEmployeeModel.SearchEmployees1(company_hash, department, jobtitle, employee_name);//只輸入一個篩選條件

            ViewBag.all_passemployee = all_passEmployees;//全部的已審核資料
            ViewBag.department = departments;//部門名稱
            ViewBag.jobtitle = jobtitles;//職稱
            ViewBag.review_employee = reviewEmployees;//待審核資料   
            ViewBag.pass_employee = searchEmployees; //篩選後的審核過資料
            return View("Index");
        }
    }
}