using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace groupme.Controllers {
    [ApiController]
    [Route("/")]
    public class GroupMeController: ControllerBase {
        private readonly ILogger<GroupMeController> _logger;
        private static readonly HttpClient client = new HttpClient();
        private string BotId;

        public GroupMeController(ILogger<GroupMeController> logger) {
            _logger = logger;
            BotId = Environment.GetEnvironmentVariable("BOT_ID");
            _logger.LogInformation($"bot_id: {BotId}");
        }

        [HttpPost]
        public ActionResult<GroupMe> Post(GroupMe gm) {
            _logger.LogInformation(JsonConvert.SerializeObject(gm, Formatting.Indented));

            // Brandon's GroupMe user_id lol
            if (gm.user_id == "19827069") {
                PostResponse(gm.name);
            }

            return Ok();
        }

        public void PostResponse(string name)
        {
            var fname = name.Split()[0];
            var json = JObject.Parse($@"{{
                bot_id: '{BotId}',
                text: 'Fuck you {fname}',
            }}");
            _logger.LogInformation(json.ToString());

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.groupme.com/v3/bots/post", content).Result;

            if (!response.IsSuccessStatusCode) {
                _logger.LogInformation("Post failed");
            }
        }
    }
}
