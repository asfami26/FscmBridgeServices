namespace FscmBridgeServices.DTOS
{
    public class CommonResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T data { get; set; }
    }
}

