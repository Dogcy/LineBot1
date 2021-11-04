using LineBot.Repository;
using LineBot.Repository.Models;
using System.Collections.Generic;
using System.Linq;
namespace LineBot.Services.Bookkeep
{
    public class BookKeep
    {
        private readonly LineDbContext _db;
        public BookKeep(LineDbContext lineDbContext)
        {
            _db = lineDbContext;
        }
        public string Pay(int money, string record)
        {

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
