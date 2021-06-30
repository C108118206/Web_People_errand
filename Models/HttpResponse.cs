using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AttendanceManagement.Models
{
    public class HttpResponse
    {
        public static HttpClient client = new HttpClient();
        public static HttpResponseMessage response = new HttpResponseMessage();
        public static readonly string url = "http://163.18.110.100/api/"; //主機的URL
        public static readonly string CompanyGetWorkRecord = "Companies/GetWorkRecord/"; //查看公司員工打卡紀錄api

        public static string GetResponse { get; set; } //存取API回傳的內容

    }
}