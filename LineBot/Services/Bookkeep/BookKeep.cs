using LineBot.Repository;
using LineBot.Repository.Models;
using System.Collections.Generic;
using System.Linq;
namespace LineBot.Services.Bookkeep
{
    public class BookKeep
    {
        private readonly LineDbContext _db;
        private readonly int _money;
        private readonly string _description;
        public BookKeep(LineDbContext lineDbContext,string text)
        {
            _db = lineDbContext;


        }
        public void Parsing(string instructionText)
        {

        }
        public string Pay(int money , string description)
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
