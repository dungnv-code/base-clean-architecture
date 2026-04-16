#nullable disable

namespace Application.DTOs.Common;

public class MetaDataDTO
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPage => PageSize > 0 ? (int)Math.Ceiling((double)Total / PageSize) : 0;
}

public class BaseResponseDTO<T>
{
    public int Code { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public MetaDataDTO MetaData { get; set; }

    public static BaseResponseDTO<T> SuccessResponse(T data, string message = "Success", MetaDataDTO metaData = null)
    {
        return new BaseResponseDTO<T>
        {
            Code = 200,
            Success = true,
            Message = message,
            Data = data,
            MetaData = metaData
        };
    }

    public static BaseResponseDTO<T> FailResponse(string message, int code = 400)
    {
        return new BaseResponseDTO<T>
        {
            Code = code,
            Success = false,
            Message = message,
            Data = default
        };
    }
}
