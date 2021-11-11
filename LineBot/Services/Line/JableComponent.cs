using LineBot.Services.Jable;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LineBot.Repository;
using LineBot.Repository.Models;

namespace LineBot.Services.Line
{
    public class JableComponent
    {
        private readonly LineDbContext _db;
        public JableComponent(LineDbContext lineDbContext)
        {
            _db = lineDbContext;
        }
        public string SerchVideosComponent(string instructionText, int uid)
        {
            this.JableRecord(instructionText, uid);
            JableVideos jable = new JableVideos();
            Task<List<JableModel>> JableModels = jable.GetJableVideos(instructionText);
            var JableVideos = JableModels.Result;
            string result = string.Empty;
            if (JableVideos.Count == 0)
            {
                return "找不到影片請游子瑩拍";
            }
            JableVideos.Select(c => result += c.VideoName + "\n" + c.VideoLink + "\n").ToList();
            return result;
        }
        private void JableRecord(string instructionText, int uid)
        {
            string serchWord = string.Empty;
            var splitSerchWord = instructionText.Split(" ");
            if (splitSerchWord.Length != 1)
            {
                serchWord = splitSerchWord[1];
            }                     
            var jableRecord = new JableRecord()
            {
                Uid = uid,
                SerchWord = serchWord,
                CreateTime = LineBot.Infrastructure.DateTimeExtension.TaipeiNow(),                
            };
            _db.JableRecords.Add(jableRecord);
            _db.SaveChanges();
        }
    }
}
