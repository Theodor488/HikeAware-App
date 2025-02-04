using HikeAware.API.Models;
using HikeAware.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HikeAware.API.Controllers
{
    [Route("chatbot")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("query")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest chatRequest)
        {
            if (string.IsNullOrEmpty(chatRequest.Message))
            {
                return BadRequest(new { error = "Chat Request Message was empty" });
            }

            var response = await _chatbotService.GetChatbotResponseAsync(chatRequest.Message);
            return Ok(new {response });
        }
    }
}
