using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Response
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
