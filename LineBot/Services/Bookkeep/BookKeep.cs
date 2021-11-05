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
        public string Parsing(string instructionText,int userId)
        {
            bool success;
            string result = string.Empty;
            var list = instructionText.Split(" ");
            var type = list.Length;
            success = int.TryParse(list[1], out _money);

            if (success)
            {             
                switch (type)
                {
                    case 1:
                        if (instructionText[1] == '月' || instructionText[1] == '日')
                        {
                            result = CheckPayRecord();
                        }
                        break;
                    case 2:
                        result = Pay(_money,userId);
                        break;
                    case 3:
                        if (success)
                        {
                            result = Pay(_money, list[2], userId);
                        }
                        break;
                }
            }

            return result;

        }
        /// <summary>
        /// 記帳
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public string Pay(int money,int userId)
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
        public string Pay(int money, string description,int userId)
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
        public string CheckPayRecord()
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
