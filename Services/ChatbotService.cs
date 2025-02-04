using HikeAware.API.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HikeAware.API.Services
{
    public class ChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ChatbotService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetChatbotResponseAsync(string message)
        {
            const string requestUri = "https://api.openai.com/v1/chat/completions";
            string openAiApiKey = _configuration["OpenAI:ApiKey"];

            if (string.IsNullOrEmpty(openAiApiKey))
            {
                throw new Exception("OpenAI Api Key is missing.");
            }

            // Format a JSON request body for OpenAI API
            var requestBody = new
            {
                temperature = 0.2,
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiApiKey);

            // Create an HTTP request to OpenAI API and send it
            var response = await _httpClient.PostAsync(
                requestUri,
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));


            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API Error: {response.StatusCode} - {errorContent}");
            }

            // Extract the chatbot's response from the API's response JSON
            var responseBody = JsonConvert.DeserializeObject<ResponseBodyModel>(await response.Content.ReadAsStringAsync());
            return responseBody.Choices[0].Message.Content.Trim();
        }
    }
}
