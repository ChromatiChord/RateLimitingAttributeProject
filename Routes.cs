using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            var data = new { email = "email@email.com" };
            return Ok(data);
        }

        [HttpPost]
        public ActionResult Post()
        {
            return Ok();
        }
    }
}
