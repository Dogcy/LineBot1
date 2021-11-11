using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isRock.LineBot;
using System.IO;
using System.Text.Json;
using LineBot.Propertys;
using Microsoft.Extensions.Configuration;
using LineBot.Services.WeatherInformation;
using LineBot.Services.Line;
using AngleSharp;
using System.Net.Http;
using LineBot.Services.LOL;
using LineBot.Services.EnglishDictionary;
using LineBot.Services.Youtube;
using LineBot.Repository;
using LineBot.Services.Bookkeep;

namespace LineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotApi : ControllerBase
    {
        public readonly CheckMember _mb;
        public readonly BookKeep _bk;
        private readonly JableComponent _jb;
        public LineBotApi(BookKeep bookKeep, CheckMember checkMember,JableComponent jableComponent)
        {
            _bk = bookKeep;
            _mb = checkMember;
            _jb = jableComponent;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
        /// <summary>
        /// 接收Line訊息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string messageText = "";
            string replyMessage = "";
            isRock.LineBot.Bot bot;

            // 資料存在appsettings
            var ChannelAccessToken = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build()["LineBot:ChannelAccessToken"];

            //create bot instance
            bot = new isRock.LineBot.Bot(ChannelAccessToken);
            //string postData = Request.Content.ReadAsStringAsync().Result;
            Request.EnableBuffering();
            Request.Body.Position = 0;
            var jsonString = new StreamReader(Request.Body).ReadToEndAsync().Result;
            var result = isRock.LineBot.Utility.Parsing(jsonString);
            var resultEvents = result.events.FirstOrDefault();
            var replyToken = resultEvents.replyToken;
            messageText = resultEvents.message.text;
            int userId;

            //  圖片的話message.type==s

            if (JudgeMessageType.CheckIsCallBot(resultEvents.message.type, messageText))
            {
                var typeText = messageText.Split(" ")[0].ToLower();
                ICarouselComponent carouselComponent = null;
                var checkIsDollar = typeText.FirstOrDefault(c => c == '$');
                if (checkIsDollar == '$')
                { typeText = "$"; }
                switch (typeText)
                {
                    case "@天氣":
                        //因push要收費改為reply
                        var we = new WeatherComponent();
                        //CarouselTemplate carouselWeatherTemplateData = carouselComponent.Component(messageText);
                        //JudgeMessageType.RespondsCarouselTemplateData(bot, resultEvents, carouselWeatherTemplateData);              // 使用carouselTemplate無法用Reply功能
                        var weatherData = we.ReplyWeatherInfo(messageText);
                        replyMessage = weatherData;
                        break;

                    case "@戰績":
                        carouselComponent = new LOLComponent();
                        CarouselTemplate carouselLoLdata = carouselComponent.Component(messageText);
                        JudgeMessageType.RespondsCarouselTemplateData(bot, resultEvents, carouselLoLdata);
                        replyMessage = "別查了你最雷";
                        break;
                    case "@e":
                        EnglishDictionary englishDictionary = new EnglishDictionary();
                        var searchExplain = englishDictionary.SerchEnglishAsync(messageText).Result;
                        replyMessage = searchExplain;
                        break;
                    case "@yt":
                        Youtube yt = new Youtube();
                        var youtubeData = yt.YoutubeSearch(messageText).Result;
                        //JudgeMessageType.RespondsStr(bot, resultEvents, youtubeData)
                        replyMessage = youtubeData;
                        break;
                    case "@自己去大便":          // 離開
                        replyMessage = "我自己搭電梯";
                        bot.Leave(resultEvents.source.groupId);
                        break;
                    case "$":
                         userId = _mb.CheckIsMember(resultEvents);
                        replyMessage = _bk.Parsing(messageText, userId);
                        break;
                    case "@jab":
                         userId = _mb.CheckIsMember(resultEvents);

                        replyMessage = _jb.SerchVideosComponent(messageText,userId);
                        break;
                    case "@help":
                        string help = "請使用@來指定功能\n\n 查詢天氣請用:@天氣\n 查詢指定天氣請用:@天氣 臺北市\n 查詢LOL戰績請用:@戰績 LolID \n查詢英文單字請用:@e word \n 給開發者:@ToDeveloper message.. \n離開房間:@自己去大便 " + "\n\n近期新功能\n查詢youtube請用: @yt video\n";
                        string bookKeepHelp = "記帳功能:\n查詢兩日內消費紀錄:$日\n查詢上週與本週消費紀錄:$週\n\n記帳指令1:$512\n記帳指令2:$512 吃飯";
                        replyMessage = help + "\n\n" + bookKeepHelp;
                        break;
                    case "@todeveloper":
                        JudgeMessageType.RespondsException(bot, resultEvents);
                        replyMessage = "收到~";
                        break;
                    case "@長宇":
                        replyMessage = "是狗";
                        break;
                    case "@杉杉":
                        replyMessage = "我也要油雞燒鴨";
                        break;
                    case "@練兵creek":
                        replyMessage = "是大屌男";
                        break;
                    case "@阿曼達":
                        replyMessage = "瘋子";
                        break;

                }
            }
            else
            {
                return Ok();
            }

            try
            {
                bot.ReplyMessage(replyToken, replyMessage);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {

                if(ex.Message== "訊息內容太長")
                {
                    bot.ReplyMessage(replyToken, "錯誤:文字訊息內容太長>想辦法自己關鍵字更準確的搜尋摟 我懶得改");
                }               
                return Ok();
            }
        }

    }
}
