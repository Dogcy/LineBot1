
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using LineBot.Repository;

namespace LineBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DiTestController : ControllerBase
    {
        private readonly LineDbContext _db;

        public DiTestController(LineDbContext lineDbDbContext)
        {
            _db = lineDbDbContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var s = _db.Users.Select(c => c).ToList();
            return Ok(s);
        }
    }
}
