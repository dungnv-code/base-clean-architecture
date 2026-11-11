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

public static class UserMessage
{
    public const string NOT_FOUND = "Không tìm thấy người dùng!";
    public const string NAME_REQUIRED = "Tên ngươi dùng là bắt buộc!";
    public const string EMAIL_REQUIRED = "Email là bắt buộc!";
    public const string PHONE_NUMBER_REQUIRED = "Số điện thoại là bắt buộc!";
    public const string ADDRESS_REQUIRED = "Địa chỉ là bắt buộc!";
    public const string AVATAR_REQUIRED = "Avata là bắt buộc!";
    public const string NOT_PASS = "Mật khẩu chưa chính xác!";
}
