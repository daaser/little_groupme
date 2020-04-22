using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace groupme.Controllers
{
    [ApiController]
    [Route("/arb")]
    public class ArbitraryController: ControllerBase
    {
        private readonly ILogger<ArbitraryController> _logger;

        public ArbitraryController(ILogger<ArbitraryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post(JsonElement j)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            _logger.LogInformation(JsonSerializer.Serialize(j, options));
            return Ok();
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("bad dog\n");
        }
    }
}
