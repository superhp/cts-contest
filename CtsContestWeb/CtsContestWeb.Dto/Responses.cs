namespace CtsContestWeb.Dto
{
    public class EmptyResponse
    {
        public EmptyResponse()
        {
            IsSuccess = true;
        }

        public EmptyResponse(string message)
        {
            Message = message;
            IsSuccess = true;
            ShowMessage = true;
        }

        public EmptyResponse(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
            ShowMessage = true;
        }

        public bool IsSuccess { set; get; }
        public bool ShowMessage { set; get; }
        public string Message { set; get; }
    }

    public class Response<TModel> : EmptyResponse
        where TModel : class
    {
        public Response()
        {
        }

        public Response(string message) : base(message)
        {
        }
        public Response(string message, bool isSuccess) : base(message, isSuccess)
        {
        }

        public TModel Data { set; get; }
    }
}