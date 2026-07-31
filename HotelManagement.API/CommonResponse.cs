using System.ComponentModel.DataAnnotations;

namespace HotelManagement.API
{
    public class CommonResponse
    {
        public CommonResponse()
        {
            
        }

        public CommonResponse(string message, object result, bool isSuccess, int httpStatusCode)
        {
            Message = message;
            Result = result;
            HttpStatusCode = httpStatusCode;
            IsSuccess = isSuccess;
        }

        public string Message { get; set; }
        public int HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }

    public static class CommonResponseMessage
    {
        public static string SuccessMessage { get; } = "Request Processed successfully";
    }
}
