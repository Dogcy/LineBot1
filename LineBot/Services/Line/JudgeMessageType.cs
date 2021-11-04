using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineBot.Services.Line
{
    public static class JudgeMessageType
    {
       
         private static readonly string _myLineID="Uaab45be7edd7ddda9ceb96963b51d11a";  // C.Y LINE ID
        /// <summary>
        /// 是否為圖片or判斷文字
        /// </summary>
        /// <param name="messageType">圖片type</param>
        /// <param name="userMessage">文字</param>
        /// <returns></returns>
        public static bool CheckIsCallBot( string messageType, string userMessage)
        {
            // user傳遞圖片會是sticker
            if (messageType != "sticker" && (userMessage.Substring(0, 1) == "@" || userMessage.Substring(0, 1) =="$"))
            {
                return true;
            }
            return false;
              
        }
        /// <summary>
        /// 回覆CarouselTemplate資料訊息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="e"></param>
        public static void RespondsCarouselTemplateData(Bot bot ,Event e, CarouselTemplate carouselTemplate)
        {
            // user傳遞圖片會是sticker
            if (e.source.groupId != null)
            {
                bot.PushMessage(e.source.groupId, carouselTemplate);
            }
            else if (e.source.roomId != null)
            {
                bot.PushMessage(e.source.roomId, carouselTemplate);
            }
            else
            {
                bot.PushMessage(e.source.userId, carouselTemplate);
            }
       

        }

        public static void RespondsStr(Bot bot,Event e,string str)
        {
            // user傳遞圖片會是sticker
            if (e.source.groupId != null)
            {
                bot.PushMessage(e.source.groupId, str);
            }
            else if (e.source.roomId != null)
            {
                bot.PushMessage(e.source.roomId, str);
            }
            else
            {
                bot.PushMessage(e.source.userId, str);
            }


        }

        /// <summary>
        /// 給開發者訊息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="e"></param>
        /// <param name="message">使用者的留言</param>
        public static void RespondsException(Bot bot, Event e)
        {
            if (e.source.groupId != null)
            {
                bot.PushMessage(_myLineID, "留言內容:"+ e.message.text + "--------id=" + e.source.groupId);
            }
            else if (e.source.roomId != null)
            {
                bot.PushMessage(_myLineID, "留言內容:" + e.message.text + "--------id=" + e.source.roomId);
            }
            else
            {
                bot.PushMessage(_myLineID, "留言內容:" + e.message.text + "--------id=" + e.source.userId);
            }

        }
        /// <summary>
        /// 傳送錯誤內容給開發者
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="e"></param>
        /// <param name="ex"></param>
        public static void RespondsException(Bot bot, Event e , Exception ex)
        {
            // user傳遞圖片會是sticker
            //我的id
            if (e.source.groupId != null)
            {
                bot.PushMessage(_myLineID, ex.Message + "使用type為" + e.source.type + "id=" + e.source.groupId);
            }
            else if (e.source.roomId != null)
            {
                bot.PushMessage(_myLineID, ex.Message + "使用type為" + e.source.type + "id=" + e.source.roomId);
            }
            else
            {
                bot.PushMessage(_myLineID, ex.Message + "使用type為" + e.source.type + "id=" + e.source.userId);
            }


        }

    }
  

}






 //if (resultEvents.message.type != "sticker" && messageText.Substring(0, 1) == "@")
 //           {
