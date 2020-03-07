using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ActionResult<GroupMe> Post(GroupMe gm) {
            _logger.LogInformation(JsonConvert.SerializeObject(gm, Formatting.Indented));

            if (gm.user_id == _config["Id"]) {
                PostResponse(gm.name);
            }

            return Ok();
        }

        public void PostResponse(string name) {
            var fname = name.Split()[0];
            var bot_id = _config["BotId"];
            var json = JObject.Parse($@"{{
                bot_id: '{bot_id}',
                text: 'Bitch spotted: {fname}',
            }}");
            _logger.LogInformation(json.ToString());

            var client = _clientFactory.CreateClient("groupme");
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var response = client.PostAsync("v3/bots/post", content).Result;

            if (!response.IsSuccessStatusCode) {
                _logger.LogInformation("Post failed");
            }
        }
    }
}
