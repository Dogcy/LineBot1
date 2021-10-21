using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isRock.LineBot;
using LineBot.Propertys;
using LineBot.Services.WeatherInformation;
namespace LineBot.Services.Line
{
    public class WeatherComponent : ICarouselComponent
    {
        // lineBot回訊息有些無法用reply方法來回應
        // 需用userId 或groupId 做messagePush方法回應

        /// <summary>
        /// 輪播天氣資訊
        /// </summary>
        /// <param name="area"></param>
        /// <returns>暫時禁止使用</returns>
        public CarouselTemplate Component(string instructionText)
        {
            var weatherInfo = new WeatherInfo();

            var weatherInformationModels = weatherInfo.GetWeatherInfo(instructionText);
            var columns = new List<Column>();

            foreach (var model in weatherInformationModels)
            {

                var col = new Column()   // 最多只能10個Column
                {

                    title = model.Loactionname,
                    text = model.Weathdescrible + " 降雨機率:" + model.Pop + "%" + "最低溫度:" + model.Mintemperature + "°c" + " 最高溫度:" + model.Maxtemperature + "°c",


                    thumbnailImageUrl = new Uri("https://arock.blob.core.windows.net/blogdata201803/29-101326-d653db4b-44ea-4fe9-af6b-26730734d450.png"),
                    actions = new List<TemplateActionBase>() { new MessageAction() { label = " ", text = " " } }
                };
                columns.Add(col);
            }
            var columns10 = columns.Take(10);
            //var ImageCarouselTemplate = new isRock.LineBot.CarouselTemplate();


            var carouselTemplate = new isRock.LineBot.CarouselTemplate();
            carouselTemplate.columns = columns10.ToList();
            return carouselTemplate;
        }


        public string ReplyWeatherInfo(string instructionText)
        {
            var weatherInfo = new WeatherInfo();

            var model = weatherInfo.GetOneWeatherInfo(instructionText);


            var data = model.Loactionname + "\n" + model.Weathdescrible + " 降雨機率:" + model.Pop + "%" + "最低溫度:" + model.Mintemperature + "°c" + " 最高溫度:" + model.Maxtemperature + "°c",
                return data;

        }









        /// <summary>
        /// ButtonsTemplate
        /// 範例
        /// </summary>
        /// <returns></returns>
        public ButtonsTemplate ButtonsTemplate()
        {
            var act1 = new isRock.LineBot.MessageAction();
            var tmp = new isRock.LineBot.ButtonsTemplate()
            {
                text = "Button Template text",
                title = "Button Template title",
                thumbnailImageUrl = new Uri("https://i.imgur.com/wVpGCoP.png"),

            };

            tmp.actions.Add(act1);
            return tmp;
        }


        /// <summary>
        /// TextMessage-Demo 
        /// 範例
        /// </summary>
        public MessageBase textMessage()
        {
            isRock.LineBot.MessageBase responseMsg = null;
            List<isRock.LineBot.MessageBase> responseMsgs =
         new List<isRock.LineBot.MessageBase>();
            responseMsg = new isRock.LineBot.TextMessage($"None handled event type : okokok");
            responseMsgs.Add(responseMsg);
            responseMsgs.Add(new isRock.LineBot.TextMessage($"None handled event type : okokok2"));

            return responseMsg;
        }


        /// <summary>
        /// ImageCarouselColumn
        /// 範例
        /// </summary>
        /// <returns></returns>
        public ImageCarouselColumn ImageCarouselColumn()
        {
            //第一個Column 
            var ImageCarouselColumn1 = new isRock.LineBot.ImageCarouselColumn
            {
                //設定圖片
                imageUrl = new Uri("https://arock.blob.core.windows.net/blogdata201706/22-124357-ad3c87d6-b9cc-488a-8150-1c2fe642d237.png"),
                //設定回覆動作
                action = new isRock.LineBot.MessageAction() { label = "標題A", text = "回覆文字A" }

            };
            //第一個Column 
            var ImageCarouselColumn2 = new isRock.LineBot.ImageCarouselColumn
            {
                //設定圖片
                imageUrl = new Uri("https://arock.blob.core.windows.net/blogdata201803/29-101326-d653db4b-44ea-4fe9-af6b-26730734d450.png"),
                //設定回覆動作
                action = new isRock.LineBot.MessageAction() { label = "標題B", text = "回覆文字B" }
            };
            return ImageCarouselColumn1;
        }

    }
}
