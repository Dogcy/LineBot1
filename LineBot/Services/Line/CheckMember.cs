using isRock.LineBot;
using LineBot.Repository;
using LineBot.Repository.Models;
using System.Linq;
namespace LineBot.Services.Line
{
    public class CheckMember
    {
        public readonly LineDbContext _db;
        public CheckMember(LineDbContext lineDbContext)
        {
            _db = lineDbContext;
        }
        public int CheckIsMember(Event e)
        {
            var lineId = _db.Users.FirstOrDefault(c => c.LineId == e.source.userId);
            int userId;
            if (lineId == null)
            {
                var user = new User()
                {
                    LineId = e.source.userId,
                    CreateTime = LineBot.Infrastructure.DateTimeExtension.TaipeiNow(),
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                userId = user.Id;
            }
            else
            {
                userId = lineId.Id;
            }
            return userId;
        }
    }
}
