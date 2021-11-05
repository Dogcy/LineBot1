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
        public void CheckIsMember(Event e)
        {
            var lineId = _db.Users.FirstOrDefault(c => c.LineId == e.source.userId);
            if (lineId == null)
            {
                _db.Users.Add(
                    new User()
                    {
                        LineId=e.source.userId,
                
                    }
                    );
            }
        }
    }
}
