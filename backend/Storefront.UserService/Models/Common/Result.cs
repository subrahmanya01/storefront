namespace Storefront.UserService.Models
{
    public class Result<T> : IResult<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }

        public string[] Errors { get; set; }

        public T? Data { get; set; }

        public Result(int status = StatusCodes.Status200OK, string message = "Success", bool isSuccess = true, T? data = default, string[]? errors = null)
        {
            Status = status;
            Message = message;
            IsSuccess = isSuccess;
            Data = data;
            Errors = errors ?? Array.Empty<string>();
        }

        public static Result<T> Ok(T data, string message = "Success", int status = StatusCodes.Status200OK)
        {
            return new Result<T>(status, message, true, data);
        }

        public static Result<T> Fail(string message, int status = StatusCodes.Status400BadRequest, params string[] errors)
        {
            return new Result<T>(status, message, false, default, errors);
        }
    }
}
