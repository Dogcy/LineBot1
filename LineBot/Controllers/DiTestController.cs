﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using LineBot.Repository;
using LineBot.Services.Bookkeep;

namespace LineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BookkeepController : ControllerBase
    {
        private readonly LineDbContext _db;
        private readonly BookKeep _bk;
        //private readonly BookKeep2 _bk2;

        // 假若bookKeep2 沒有注入 則 bookKeep2裡的LineDbContext也不會有值
        public BookkeepController(LineDbContext lineDbDbContext, BookKeep2 bookKeep2,BookKeep bookKeep)
        {
            _db = lineDbDbContext;
          
            _bk = bookKeep;
       
        }
        [HttpGet]
        [Route("test/test")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
