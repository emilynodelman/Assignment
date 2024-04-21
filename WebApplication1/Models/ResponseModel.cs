using System.Net;
namespace WebApplication1.Models
{
    public class ResponseModel<T>
    {
        public List<ErrorModel> Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public T Content { get; set; }
    }

    public class ErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }

        public ErrorModel(string code, string message, string additionalInfo)
        {
            Code = code;
            Message = message;
            AdditionalInfo = additionalInfo;
        }
    }
}
