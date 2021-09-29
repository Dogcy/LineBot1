using isRock.LineBot;
using LineBot.Propertys;
using LineBot.Services.LOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineBot.Services.Line
{
    public class LOLComponent : ICarouselComponent
    {
        private CarouselTemplate _component;
        public CarouselTemplate Component(string instructionText)
        {
            LOLRecord lolRecord = new LOLRecord();
            Task<List<LOLModel>> LoLDataAsync = lolRecord.getLolRecordAsync(instructionText);
            var loldata= LoLDataAsync.Result;
            var columns = new List<Column>();

            foreach (var model in loldata)
            {

                var col = new Column()   // 最多只能10個Column
                {

                    title = model.Victory,
                    text = model.Data, //無法放超過60


                    thumbnailImageUrl = new Uri(model.RoleImage),
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
    }
}
