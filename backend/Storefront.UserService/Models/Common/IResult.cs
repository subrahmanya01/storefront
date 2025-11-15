namespace Storefront.UserService.Models
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        int Status { get; set; }
        string[] Errors { get; set; }
    }
    public interface IResult<T> : IResult
    {
        T? Data { get; set; }
    }
}
