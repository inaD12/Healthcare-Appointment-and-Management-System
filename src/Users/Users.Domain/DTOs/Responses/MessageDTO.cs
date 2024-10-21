namespace Users.Domain.DTOs.Responses
{
    public class MessageDTO
    {
        public string Message { get; set; }

        public MessageDTO(string message)
        {
            Message = message;
        }
    }
}
