namespace VenueService.common
{
    public class ResponseBody
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public ResponseBody(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ResponseBody(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

    }
}
