using LineBot.Infrastructure;
using LineBot.Repository;
using LineBot.Repository.Models;
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
        public string Parsing(string instructionText)
        {
            bool success;
            string result = string.Empty;
            var list = instructionText.Split(" ");
            var type = list.Length;
            switch (type)
            {
                case 1:
                    if (instructionText[1] == '月' || instructionText[1] == '日')
                    {
                        result = CheckPayRecord();
                    }
                    break;
                case 2:
                    success = int.TryParse(list[1], out _money);

                    if (success)
                    {
                        result = Pay(_money);
                    }
                    break;
                case 3:
                    success = int.TryParse(list[1], out _money);

                    if (success)
                    {
                        result = Pay(_money, list[2]);
                    }
                    break;
            }
            return result;

        }
        public string Pay(int money)
        {
            var consumingRecords = new ConsumingRecord()
            {
                CreateTime = DateTimeExtension.TaipeiNow(),
                Uid = 2,
                Price = money,
                Description = "無"
            };
            _db.ConsumingRecords.Add(consumingRecords);
            _db.SaveChanges();
            return money + "$ 已記錄";
        }
        public string Pay(int money, string description)
        {
            var consumingRecords = new ConsumingRecord()
            {
                CreateTime = DateTimeExtension.TaipeiNow(),
                Uid = 2,
                Price = money,
                Description = description
            };
            _db.ConsumingRecords.Add(consumingRecords);
            _db.SaveChanges();
            return "消費:" + money + "$" + "備註:" + description;
        }
        public string CheckPayRecord()
        {
            _db.
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
