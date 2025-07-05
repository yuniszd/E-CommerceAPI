namespace E_CommerceAPI.Application.Shared;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public BaseResponse() { }

    public BaseResponse(bool success, string message, T? data = default, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }
}
