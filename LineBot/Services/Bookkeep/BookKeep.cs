using LineBot.DTO;
using LineBot.Infrastructure;
using LineBot.Repository;
using LineBot.Repository.Models;
using LineBot.Services.Line;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineBot.Services.Bookkeep
{
    public class BookKeep
    {
        private readonly LineDbContext _db;
        private int _money;
        private string _description;
        public BookKeep(LineDbContext lineDbContext)
        {
            _db = lineDbContext;

        }
        public string Parsing(string instructionText, int userId)
        {
            bool success;
            string result = string.Empty;
            instructionText = instructionText.Insert(1, " ");
            var list = instructionText.Split(" ");
            var type = list.Length;
            success = int.TryParse(list[1], out _money);


            // 長度一 查紀錄
            // 二 寫入消費金額
            // 三 寫入金額及內容
            switch (type)
            {
                case 1:

                    break;
                case 2:
                    if (success)
                    {
                        result = Pay(_money, userId);
                    }
                    else
                    {
                        if (list[1] == "月" || list[1] == "日" || list[1] == "週")
                        {
                            result = CheckPayRecord(list[1].ToString(), userId);
                        }
                    }
                    break;
                case 3:
                    if (success)
                    {
                        result = Pay(_money, list[2], userId);
                    }
                    break;
            }


            return result;

        }
        /// <summary>
        /// 記帳
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        private string Pay(int money, int userId)
        {
            var consumingRecords = new ConsumingRecord()
            {
                CreateTime = DateTimeExtension.TaipeiNow(),
                Uid = userId,
                Price = money,
                Description = "無"
            };
            _db.ConsumingRecords.Add(consumingRecords);
            _db.SaveChanges();
            return "消費:" + money + "$--無\n已記錄";
        }
        /// <summary>
        /// 記帳及備註
        /// </summary>
        /// <param name="money"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private string Pay(int money, string description, int userId)
        {
            var consumingRecords = new ConsumingRecord()
            {
                CreateTime = DateTimeExtension.TaipeiNow(),
                Uid = userId,
                Price = money,
                Description = description
            };
            _db.ConsumingRecords.Add(consumingRecords);
            _db.SaveChanges();
            return "消費:" + money + "$--"  + description + "\n已記錄";
        }



        private string CheckPayRecord(string type, int userId)
        {
            string result = string.Empty;
            switch (type)
            {
                case "日":
                    result = CheckPayRecordTodayAndYesterday(userId);
                    break;
                case "週":
                    result = CheckPayRecordWeek(userId);
                    break;
                case "月":
                    result = CheckPayRecordMonth(userId);
                    break;
            }
            return result;
        }
        private string CheckPayRecordTodayAndYesterday(int userId)
        {
            DateTime yesterday = DateTimeExtension.TaipeiNow().AddDays(-1).Date;
            var dayData = _db.ConsumingRecords
                    .Where(c => c.Uid == userId && c.CreateTime >= yesterday)
                    .AsEnumerable()
                    .GroupBy(c => new { c.CreateTime.Date })
                    .Select(group => new BookKeepModel
                    {
                        CreateTime = group.Key.Date,
                        BookKeepDetails = group.Select(s => new BookKeepDetail()
                        {
                            Description = s.Description,
                            Price = s.Price
                        }).ToList()
                    }).OrderByDescending(c => c.CreateTime).ToList();

            string result = "----今,昨日消費---\n\n";
            if (dayData.Count == 0)
            {
                result = "兩天內查無紀錄";
                return result;
            }
            foreach (var item in dayData)
            {
                result += "日期:" + item.CreateTime.Date.ToString("MM/dd")+"\n";
                result += "當日總消費金額:" + item.BookKeepDetails.Sum(c => c.Price);
                result += "\n";
                item.BookKeepDetails.Select(c => result += c.Price + "$----" + c.Description.ToString() + "\n").ToList();
                result += "\n";
            }
            return result;
        }

        private string CheckPayRecordWeek(int userId)
        {
            // 本週
            DateTime thisWeekMonday = DateTimeExtension.TaipeiNow().AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)).Date;
            DateTime lastWeekMonday = DateTime.Now.AddDays(-6 - Convert.ToInt16(DateTime.Now.DayOfWeek)).Date;
            var thisWeekData = _db.ConsumingRecords
        .Where(c => c.Uid == userId && c.CreateTime >= thisWeekMonday)
        .AsEnumerable()
          .Select(c => new BookKeepWeekModel
          {
              Day = c.CreateTime.DayOfWeek.ToString(),
              Description = c.Description,
              Price = c.Price
          }).GroupBy(c => c.Day)
          .ToList();
            var lastWeekData = _db.ConsumingRecords
        .Where(c => c.Uid == userId && c.CreateTime >= lastWeekMonday && c.CreateTime < thisWeekMonday)
                .AsEnumerable()
          .Select(c => new BookKeepWeekModel
          {
              Day = c.CreateTime.DayOfWeek.ToString(),
              Description = c.Description,
              Price = c.Price
          }).GroupBy(c => c.Day)
          .ToList();
            string result = String.Empty;
            int totalPrice;
          totalPrice=   thisWeekData.Sum(group=>group.Sum(c=>c.Price));
            result += "----本週---總消費:"+ totalPrice;
            foreach (var item in thisWeekData)
            {
                result += "\n<" + item.Key + ">";
                item.Select(c => result +="\n"+ c.Price + "$----" + c.Description.ToString()).ToList();

            }
            totalPrice = lastWeekData.Sum(group => group.Sum(c => c.Price));
            result += "\n\n\n----上週---總消費:"+ totalPrice;
            foreach (var item in lastWeekData)
            {
                result += "\n<" + item.Key + ">";
                item.Select(c => result +="\n"+ c.Price + "$----" + c.Description.ToString()).ToList();
            }

            return result;
        }
        private string CheckPayRecordMonth(int userId)
        {

            return "";
        }

    }

    /// <summary>
    /// 測試
    /// </summary>
    public class BookKeep2
    {
        private readonly LineDbContext _db;
        public BookKeep2(LineDbContext lineDbContext)
        {
            _db = lineDbContext;
        }
        public List<User> test1()
        {
            return _db.Users.ToList();
        }
    }
}
