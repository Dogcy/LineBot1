using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LineBot.Services.Jable
{
    public class JableModel
    {
        public string VideoName { get; set; }
        public string ImgUrl { get; set; }
        public string VideoLink { get; set; }
    }
    public class JableVideos
    {

        public async Task<List<JableModel>> GetJableVideos(string serchName)
        {
            serchName = serchName.Split(" ")[1];
            string url = "https://jable.tv/search/";

            if (serchName != "")
            {
                url += serchName + "/";
            }

            HttpClient httpClient = new HttpClient();

            // 原因 https://ithelp.ithome.com.tw/articles/10209356?sc=iThelpR
            //var productValue = new ProductInfoHeaderValue("User-agent", "1.1");
            //httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);
            //下面寫法同上
            httpClient.DefaultRequestHeaders.Add("User-agent", "777");             // 此網站隨便給個user-agent就會過了

            var responseMessage = await httpClient.GetAsync(url); //發送請求


            string responseResult = string.Empty;
            //檢查回應的伺服器狀態StatusCode是否是200 OK
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseResult = responseMessage.Content.ReadAsStringAsync().Result;//取得內容
            }
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(res => res.Content(responseResult));
            var title = document.Title;
            //var s =document.GetElementsByClassName("detail")[0].InnerHtml;
            var imgUrls = document.GetElementsByClassName("img-box cover-md");

            var classDetails = document.GetElementsByClassName("detail").Take(20).ToList();
            var detailsInnerHtml = classDetails.Select(c => c.InnerHtml).Take(20).ToList();
            var jableModels = new List<JableModel>();
            var pattern = "https:.+\"";
            Regex reg = new Regex(pattern);
            var pattern2 = $"[^\\n]+[^\\n]";
            Regex regg = new Regex(pattern2);


            for (int i = 0; i < classDetails.Count; i++)
            {
       
                var strr = classDetails[i].TextContent.ToString();
                var videoName = regg.Match(strr).ToString();
                //var videoName = strr.Replace("\n", "");
                var videoLink = reg.Match(classDetails[i].InnerHtml).ToString().TrimEnd('"'); 
                     var model = new JableModel()
                {
                    VideoName = videoName,
                    VideoLink = videoLink,
                    ImgUrl = ""
                };
                jableModels.Add(model);
            }
             

            return jableModels;
        }

    }
}
