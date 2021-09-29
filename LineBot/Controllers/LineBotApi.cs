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

namespace LineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotApi : ControllerBase
    {
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
            string sendMessageToUser = "";
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








            //  圖片的話message.type==s

            if (JudgeMessageType.CheckIsCallBot(resultEvents.message.type, messageText))
            {
                var TypeText = messageText.Split(" ")[0].ToLower();
                ICarouselComponent carouselComponent = null;
                switch (TypeText)
                {
                    case "@天氣":
                        carouselComponent = new WeatherComponent();
                        CarouselTemplate carouselWeatherTemplateData = carouselComponent.Component(messageText);
                        JudgeMessageType.RespondsCarouselTemplateData(bot, resultEvents, carouselWeatherTemplateData);              // 使用carouselTemplate無法用Reply功能
                        sendMessageToUser = "這是您要的天氣資訊~";
                        break;

                    case "@戰績":
                        carouselComponent = new LOLComponent();
                        CarouselTemplate carouselLoLdata = carouselComponent.Component(messageText);
                        JudgeMessageType.RespondsCarouselTemplateData(bot, resultEvents, carouselLoLdata);
                        sendMessageToUser = "旺旺";
                        break;
                    case "@e":
                        EnglishDictionary englishDictionary = new EnglishDictionary();
                        var searchExplain = englishDictionary.SerchEnglishAsync(messageText).Result;
                        JudgeMessageType.RespondsStr(bot, resultEvents, searchExplain);
                        sendMessageToUser = "喵~";
                        break;
                    case "@自己去大便":          // 離開
                        sendMessageToUser = "我不會搭電梯";
                        bot.Leave(resultEvents.source.groupId);
                        break;
                    case "@help":
                        string help = "請使用@來指定功能\n\n 查詢天氣請用:@天氣\n 查詢指定天氣請用:@天氣 臺北市\n 查詢LOL戰機請用:@戰績 LolID\n 查詢英文單字請用:@e word \n 給開發者:@ToDeveloper message.. \n離開房間:@自己去大便";
                        sendMessageToUser = help;
                        break;
                    case "@todeveloper":
                        JudgeMessageType.RespondsException(bot, resultEvents);
                        sendMessageToUser = "收到~";
                        break;
                    case "@長宇":
                        sendMessageToUser = "是狗";
                        break;
                    case "@杉杉":
                        sendMessageToUser = "我也要油雞燒鴨";
                        break;
                }
            }
            else
            {
                return Ok();
            }

            try
            {
                bot.ReplyMessage(replyToken, sendMessageToUser);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                JudgeMessageType.RespondsException(bot, resultEvents, ex);
                return Ok();
            }
        }

    }
}
