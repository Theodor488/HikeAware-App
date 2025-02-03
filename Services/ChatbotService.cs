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
        private readonly string _openAiApiKey;

        public ChatbotService(HttpClient httpClient, string openAiApiKey)
        {
            _httpClient = httpClient;
            _openAiApiKey = openAiApiKey;
        }

        public async Task<string> GetChatbotResponseAsync(IEnumerable<object> messages)
        {
            const string requestUri = "https://api.openai.com/v1/chat/completions";

            // Format a JSON request body for OpenAI API
            var requestBody = new
            {
                temperature = 0.2,
                model = "gpt-3.5-turbo",
                messages = messages.ToList()
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _openAiApiKey);

            // Create an HTTP request to OpenAI API and send it
            var response = await _httpClient.PostAsync(
                requestUri,
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            // Extract the chatbot's response from the API's response JSON
            var responseBody = JsonConvert.DeserializeObject<ResponseBodyModel>(await response.Content.ReadAsStringAsync());
            return responseBody.Choices[0].Message.Content.Trim();
        }
    }
}
