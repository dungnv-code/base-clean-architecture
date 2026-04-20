#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.EForm;

public class TResponse
{
    public int Code { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public static TResponse SuccessResponse(string message = "Success")
        => new() { Code = 200, Success = true, Message = message };

    public static TResponse FailResponse(string message, int code = 400)
        => new() { Code = code, Success = false, Message = message };
}

public class TResponse<T> : TResponse
{
    public T Data { get; set; }
    public MetaDataDTO MetaData { get; set; }

    public static TResponse<T> SuccessResponse(T data, string message = "Success", MetaDataDTO metaData = null)
        => new() { Code = 200, Success = true, Message = message, Data = data, MetaData = metaData };

    public static new TResponse<T> FailResponse(string message, int code = 400)
        => new() { Code = code, Success = false, Message = message, Data = default };
}
