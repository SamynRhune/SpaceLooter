using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Sdk
{
    public class UserRoleSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserRoleSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IdentityRole?> GetRoleFromUser(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/UserRole/GetRoleFromUser?userId={id}";


            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IdentityRole>();

            return result;
        }

        public async Task<PlayerResult?> SetRoleFromUser(string userId, string roleName)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/UserRole/SetRoleFromUser?userId={userId}&roleName={roleName}";

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlayerResult>();

            return result;
        }
    }
}
