using Newtonsoft.Json;

namespace BIZFEST_Event.Models
{
    public class ReturnStatus<T>
    {
        public ReturnStatus()
        {

        }
        public ReturnStatus(int statusCode, string message, int itemsCounts, T responseObject = default)
        {
            StatusCode = statusCode;
            Message = message;
            Count = itemsCounts;
            Result = responseObject;
        }


        [JsonProperty("statuscode")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
