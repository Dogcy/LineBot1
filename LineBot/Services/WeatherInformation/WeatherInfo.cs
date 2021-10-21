using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Services.WeatherInformation
{
    public class WeatherInformationModel
    {
        public string Loactionname { get; set; }
        public string Weathdescrible { get; set; }
        public string Pop { get; set; }
        public string Mintemperature { get; set; }
        public string Maxtemperature { get; set; }
    }
    public class WeatherInfo
    {
        /// <summary>
        /// 地區名稱   
        /// ex:@天氣 新竹市
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<WeatherInformationModel> GetWeatherInfo(string area)
        {
            string url = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?&Authorization=CWB-DAE5888F-2CE6-4DCB-A336-124E911C89F4";
            // 若有帶參數

            if (area.Split(" ").Length > 1)
            {

                url += "&locationName=" + area.Split(" ")[1];
            }
            JArray jsondata = GetWeatherInfoJson(url);
            string result = "";
            var weatherInformationModels = new List<WeatherInformationModel>();
            foreach (JObject data in jsondata)
            {
                var weatherInformationModel = new WeatherInformationModel()
                {
                    Loactionname = (string)data["locationName"], //地名
                    Weathdescrible = (string)data["weatherElement"][0]["time"][0]["parameter"]["parameterName"],//天氣狀況
                    Pop = (string)data["weatherElement"][1]["time"][0]["parameter"]["parameterName"],  //降雨機率
                    Mintemperature = (string)data["weatherElement"][2]["time"][0]["parameter"]["parameterName"], //最低溫度
                    Maxtemperature = (string)data["weatherElement"][4]["time"][0]["parameter"]["parameterName"] //最高溫度
                };
                weatherInformationModels.Add(weatherInformationModel);



            }
            return weatherInformationModels;
        }

        /// <summary>
        /// 單比格式
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public WeatherInformationModel GetOneWeatherInfo(string area)
        {
            string url = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?&Authorization=CWB-DAE5888F-2CE6-4DCB-A336-124E911C89F4";
            // 若有帶參數

            if (area.Split(" ").Length > 1)
            {

                url += "&locationName=" + area.Split(" ")[1];
            }
            JArray jsondata = GetWeatherInfoJson(url);
            var weatherInformationModel = new WeatherInformationModel()
            {
                Loactionname = (string)jsondata["locationName"], //地名
                Weathdescrible = (string)jsondata["weatherElement"][0]["time"][0]["parameter"]["parameterName"],//天氣狀況
                Pop = (string)jsondata["weatherElement"][1]["time"][0]["parameter"]["parameterName"],  //降雨機率
                Mintemperature = (string)jsondata["weatherElement"][2]["time"][0]["parameter"]["parameterName"], //最低溫度
                Maxtemperature = (string)jsondata["weatherElement"][4]["time"][0]["parameter"]["parameterName"] //最高溫度
            };
            return weatherInformationModel;
        }
        /// <summary>
        /// 與中央氣象局要資料
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private JArray GetWeatherInfoJson(string uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request請求
            req.Timeout = 10000; //request逾時時間
            req.Method = "GET"; //request方式
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
            string result = streamReader.ReadToEnd(); //讀取到最後一行
            respone.Close();
            streamReader.Close();
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
            return (JArray)jsondata["records"]["location"]; //回傳json陣列

        }

    }
}
