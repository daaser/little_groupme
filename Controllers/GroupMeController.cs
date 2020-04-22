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
using Microsoft.Extensions.Configuration;

using groupme.Models;
using groupme.Services;

namespace groupme.Controllers
{
    [ApiController]
    [Route("/")]
    public class GroupMeController: ControllerBase
    {
        private readonly ILogger<GroupMeController> _logger;
        private readonly IGroupMeService _groupme;
        private readonly IConfiguration _config;

        public GroupMeController(
            ILogger<GroupMeController> logger,
            IGroupMeService groupme,
            IConfiguration config)
        {
            _logger = logger;
            _groupme = groupme;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Post(GroupMe gm)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            _logger.LogInformation(JsonSerializer.Serialize(gm, options));
            var random = new Random();

            if (gm.user_id == _config["USER_ID"] && random.Next(4) == 1)
            {
                // var name = await _groupme.GetUserByIdAsync(gm.user_id);
                var name = "Brandon";
                var response = await _groupme.RespondToUserAsync(name);
                if (!response.IsSuccessStatusCode)
                {
                    return new StatusCodeResult((int)response.StatusCode);
                }
            }

            return Ok();
        }
    }
}
