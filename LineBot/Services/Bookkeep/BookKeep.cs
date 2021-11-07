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
            //bool success;
            string result = string.Empty;
            var list = instructionText.Split(" ");
            var type = list.Length;
            //success = int.TryParse(list[1], out _money);


            // 長度一 查紀錄
            // 二 寫入消費金額
            // 三 寫入金額及內容
            switch (type)
            {
                case 1:
                    if (instructionText[1] == '月' || instructionText[1] == '日' || instructionText[1] == '週')
                    {
                        result = CheckPayRecord(instructionText[1].ToString(), userId);
                    }
                    break;
                case 2:
                    result = Pay(_money, userId);
                    break;
                case 3:

                    result = Pay(_money, list[2], userId);

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
            return money + "$ 已記錄";
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
            return "消費:" + money + "$" + "備註:" + description;
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

            var today = DateTimeExtension.TaipeiNow().Date;
            var yesterday = DateTimeExtension.TaipeiNow().AddDays(-1).Date;
            var todayData = _db.ConsumingRecords
                    .Where(c => c.Uid == userId && c.CreateTime >= yesterday)
                    .AsEnumerable()
                    .GroupBy(c =>new { c.CreateTime.Date })
                    .Select(group => new BookKeepModel
                    {
                        CreateTime = group.Key.Date,
                        BookKeepDetails = group.Select(s => new BookKeepDetail()
                        {
                            Description = s.Description,
                            Price = s.Price
                        }).ToList()
                    }).ToList();

            return "";
        }

        private string CheckPayRecordWeek(int userId)
        {

            return "";
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
