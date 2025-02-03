namespace HikeAware.API.Models
{
    public class Message
    {
        public string Content { get; set; }
    }

    public class Choice
    {
        public Message Message { get; set; }
    }

    public class ResponseBodyModel
    {
        public List<Choice> Choices { get; set; }
    }
}
