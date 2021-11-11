using LineBot.Services.Jable;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace LineBot.Services.Line
{
    public class JableComponent
    {
        public string SerchVideosComponent(string instructionText)
        {
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
    }
}
