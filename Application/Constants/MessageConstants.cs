namespace Application.Constants;

public static class CommonMessage
{
    public const string UNAUTHORIZED = "Common_401";           // 401 - Chưa xác thực
    public const string ACCESS_DENIED = "Common_403";          // 403 - Không có quyền
    public const string NOT_FOUND = "Common_404";              // 404 - Không tìm thấy
    public const string INTERNAL_SERVER_ERROR = "Common_500";  // 500 - Lỗi server
    public const string MISSING_PARAM = "Common_501";          // 501 - Thiếu tham số
    public const string INVALID_PARAM = "Common_502";          // 502 - Tham số không hợp lệ
    public const string ALREADY_EXISTS = "Common_503";         // 503 - Dữ liệu đã tồn tại
}

public static class ChoMessage
{
    public const string NOT_FOUND = "Cho_404";
    public const string TEN_REQUIRED = "Cho_001";
    public const string DIA_CHI_REQUIRED = "Cho_002";
}