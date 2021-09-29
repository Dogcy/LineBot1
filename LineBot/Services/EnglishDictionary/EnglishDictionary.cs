using AngleSharp;
using LineBot.Propertys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LineBot.Services.EnglishDictionary
{
    public class EnglishDictionary
    {


        public async Task<string> SerchEnglishAsync(string str)
        {

            str = str.Split(" ")[1];

            HttpClient httpClient = new HttpClient();

            string url = "https://tw.dictionary.search.yahoo.com/search?p=" + str;
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

            var yahooTitel = document.Title;
            try
            {
                var searchWord = document.GetElementsByClassName("fz-24 fw-500 c-black lh-24")[0].InnerHtml;
                var searchContent = document.GetElementsByClassName("compList mb-25 p-rel")[0].QuerySelectorAll("li").ToList();
                var RespondContent = "---" + searchWord + "---\n\r";
                searchContent.Select(c => RespondContent += c.TextContent + "\n").ToList();
                return RespondContent;

            }
            catch(Exception ex)
            { 
                return "狗我查不到此字";
            }

        }
    }
}

