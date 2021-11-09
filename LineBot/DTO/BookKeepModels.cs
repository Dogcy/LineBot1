using System;
using System.Collections.Generic;

namespace LineBot.DTO
{
    public class BookKeepModel
    {
        public DateTime CreateTime { get; set; }
       public List<BookKeepDetail> BookKeepDetails { get; set; }
    }
    public class BookKeepDetail
    {
        public int Price { get; set; }
        public string Description { get; set; }
    }
    public class BookKeepWeekModel
    {
        public int Price { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
    }
}
