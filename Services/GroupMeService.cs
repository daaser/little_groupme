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

namespace groupme.Services
{
    public interface IGroupMeService
    {
        Task<string> GetUserByIdAsync(string userId);
        Task<HttpResponseMessage> RespondToUserAsync(string name);
    }

    public class GroupMeService: IGroupMeService
    {
        private readonly ILogger<GroupMeService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GroupMeService(
            ILogger<GroupMeService> logger,
            HttpClient httpClient,
            IConfiguration config)
        {
            _logger = logger;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GetUserByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync("/v3/groups/" + _config["GROUP_ID"]);
            if (!response.IsSuccessStatusCode)
            {
                return "?";
            }
            var content = await response.Content.ReadAsStringAsync();
            var group = JsonSerializer.Deserialize<GroupMeGroup>(content);

            return group.response.members.Find(m => m.user_id == userId).name;
        }

        public async Task<HttpResponseMessage> RespondToUserAsync(string name)
        {
            var fname = name.Split()[0];
            var payload = new GroupMeResponse()
            {
                bot_id = _config["BOT_ID"],
                text = $"Shut up {fname}"
            };

            var json = JsonSerializer.Serialize(payload);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("v3/bots/post", content);
        }
    }
}
