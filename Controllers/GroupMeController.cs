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

namespace groupme.Controllers {
    [ApiController]
    [Route("/")]
    public class GroupMeController: ControllerBase {
        private readonly ILogger<GroupMeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public GroupMeController(
            ILogger<GroupMeController> logger,
            IHttpClientFactory clientFactory,
            IConfiguration config
        ) {
            _logger = logger;
            _clientFactory = clientFactory;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Post(GroupMe gm) {
            var options = new JsonSerializerOptions {
                WriteIndented = true
            };
            _logger.LogInformation(JsonSerializer.Serialize(gm, options));

            if (gm.user_id == _config["Id"]) {
                var fname = gm.name.Split()[0];
                var payload = new GroupMeResponse() {
                    bot_id = _config["BotId"],
                    text = $"Shut up {fname}"
                };

                var json = JsonSerializer.Serialize(payload);

                var client = _clientFactory.CreateClient("groupme");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("v3/bots/post", content);

                if (!response.IsSuccessStatusCode) {
                    return new StatusCodeResult((int)response.StatusCode);
                }
            }

            return Ok();
        }
    }
}
