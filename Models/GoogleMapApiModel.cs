using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AttendanceManagement.Models
{
    public class GoogleMapApiModel
    {
        public static string ConvertAddressToJsonString(string address)//地址轉JSON
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + HttpUtility.UrlEncode(address, Encoding.UTF8) + "&language=zh-TW&key=AIzaSyBszSMAkbpQjbZrUSRiouvAnDlY3HZpreY";
            string result = "";//回傳結果 
            try
            {
                using (WebClient client = new WebClient())
                {
                    //指定語言，否則Google預設回傳英文   
                    client.Headers[HttpRequestHeader.AcceptLanguage] = "zh-TW";
                    //不設定的話，會回傳中文亂碼
                    client.Encoding = Encoding.UTF8;
                    result = client.DownloadString(url);
                }//end using
                return result;
            }
            catch (Exception)
            {
                result = "查無此位置";
                return result;
            }
        }//end method

        public static double[] ChineseAddressToLatLng(string json)
        {
            //回傳結果
            double[] latLng = { 0, 0 };
            if (!json.Equals("查無此位置"))
            {
                //將Json字串轉成物件  
                GoogleGeocodingAPI.RootObject rootObj = JsonConvert.DeserializeObject<GoogleGeocodingAPI.RootObject>(json);
                //防呆
                if (rootObj.status == "OK")
                {
                    //從results開始往下找 
                    double lat = rootObj.results[0].geometry.location.lat;//緯度  
                    double lng = rootObj.results[0].geometry.location.lng;//經度   
                                                                          //緯度
                    latLng[0] = lat;
                    //經度
                    latLng[1] = lng;
                }//end if 
            }
            return latLng;
        }//end method 
        public static string latLngToChineseAddress(params double[] latLng)
        {
            string url =
                "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + string.Join(",", latLng) + "&language=zh-TW&key=AIzaSyAnwBmNoOCbLM69TsWonLV-i134H4Y3o70";
            string json = String.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //指定語言，否則Google預設回傳英文 
            request.Headers.Add("Accept-Language", "zh-tw");
            try
            {
                using (var response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        json = sr.ReadToEnd();
                    }
                }
                GoogleGeocodingAPI.RootObject rootObj = JsonConvert.DeserializeObject<GoogleGeocodingAPI.RootObject>(json);

                return rootObj.results[0].formatted_address;
            }
            catch (Exception)
            {
                string result = "座標(" + string.Join(",", latLng) + ")，查無此位置";
                return result;
            }

        }
    }
}

namespace GoogleGeocodingAPI
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }
    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }
    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public bool partial_match { get; set; }
        public List<string> types { get; set; }
    }
    public class RootObject
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

}