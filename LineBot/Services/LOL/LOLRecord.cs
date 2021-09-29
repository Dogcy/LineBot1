using AngleSharp;
using LineBot.Propertys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
namespace LineBot.Services.LOL
{
    public class LOLRecord
    {

        public async Task<List<LOLModel>> getLolRecordAsync(string str)
        {

            var LOLid = str.Split(" ")[1];

            HttpClient httpClient = new HttpClient();

            string url = "https://lol.moa.tw/summoner/show/" + LOLid;
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

            var checkHasPlayer = document.Title;
            if (checkHasPlayer == "LOL戰績網")
            {

                return new List<LOLModel>() {
                    new LOLModel() { 
                        Victory="??",
                        Data="無此帳號或伺服器爆炸",
                        RoleImage="https://upload.wikimedia.org/wikipedia/commons/f/f0/Error.svg",
                } };
            }
            var outerHtml = document.GetElementById("tabs").QuerySelector("a[data-reload]").OuterHtml;     // 取得 <a href="#tabs-recentgames" data-url="/Ajax/recentgames/4180483" data-reload="true" data-cooldown="10000" data-toggle="tab">近期對戰</a>
            var NumberID = outerHtml.Split(" ")[2].Split("/")[3].Trim('"');  
            var RecordUrl = "https://lol.moa.tw/Ajax/recentgames/" + NumberID;
            //------------------------------------
            // -等待更新資料  (猜測可能在搜尋lol帳號時做更新對戰紀錄的歷史 否則可能抓到之前的資料)
            Thread.Sleep(3000);
            //------------------------------------
            Console.WriteLine(RecordUrl);
            responseMessage = await httpClient.GetAsync(RecordUrl);
            responseResult = string.Empty;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseResult = responseMessage.Content.ReadAsStringAsync().Result;//取得內容
            }
            config = Configuration.Default;
            context = BrowsingContext.New(config);
            document = await context.OpenAsync(res => res.Content(responseResult));
            var getImageDiv = document.GetElementsByClassName("champion60"); //<div class="inline-block champion champion60 championtip champion-circle" style="background-image: url(/img/lol-info/champions/tile/XinZhao_Splash_Tile_0.jpg);" data-code="XinZhao" data-original-title="" title=""><div style="color:white;font-size:x-large;position:relative;left:15px;top:30px;">18</div></div>
            var getWinLoseDiv = document.QuerySelectorAll("th[colspan]");//抓th標籤有colspan的資料

            var pattern = @"/img.+\.jpg";
            var lolRecord = new List<LOLModel>();
            for (int i = 0; i < 10; i++)
            {

                var strArray = getWinLoseDiv[i].TextContent.Replace("-", string.Empty).Replace("\n", string.Empty).Split("|").ToList();
                var removeVictory = strArray.Skip(3).Take(4).ToList();

                string data = "";
                removeVictory.Select(c => { data += c; Console.WriteLine(c); return 0; }).ToList();


                Match match = Regex.Match(getImageDiv[i].OuterHtml, pattern);
                lolRecord.Add(new LOLModel()
                {
                    RoleImage = "https://lol.moa.tw" + match.Value,
                    Victory = strArray[1], //取勝負
                    Data = data
                });
            }
            return lolRecord;

        }

    }
}
