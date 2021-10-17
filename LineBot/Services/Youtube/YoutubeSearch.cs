using LineBot.Propertys;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LineBot.Services.Youtube
{
    public class Youtube
    {//
        public async Task<string> YoutubeSearch(string str)
        {
            str = str.Split(" ")[1];

            HttpClient httpClient = new HttpClient();
            var ApiKey = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json").Build()["GoogleApi:GYoutubeApiKey"];

            string url = "https://www.googleapis.com/youtube/v3/search?type=video&key="+ ApiKey + "&part=snippet&q=" + str;
            var responseMessage = await httpClient.GetAsync(url); //發送請求

            string responseResult = string.Empty;
            //檢查回應的伺服器狀態StatusCode是否是200 OK
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseResult = responseMessage.Content.ReadAsStringAsync().Result;//取得內容
            }
            string ytVideo = "http://www.youtube.com/embed/";
            Root root = JsonConvert.DeserializeObject<Root>(responseResult);
            var data=  root.items.Select(c => new GetYT { VideoUrl = ytVideo+c.id.videoId, ImgUrl = c.snippet.thumbnails.medium.url , Title =c.snippet.title}).ToList();
            string toStr = string.Empty;
              data.Select(c => toStr += c.Title + "\r\n" +  c.VideoUrl + "\r\n"+"----------"+"\r\n" ).ToList();
            try
            {
  
                return toStr;

            }
            catch (Exception ex)
            {
                return toStr;
            }
        }
    }
}
