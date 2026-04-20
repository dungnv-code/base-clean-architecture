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

public static class KhuVucMessage
{
    public const string NOT_FOUND = "KhuVuc_404";
    public const string TEN_REQUIRED = "KhuVuc_001";
    public const string LOAI_REQUIRED = "KhuVuc_002";
}

public static class KiotMessage
{
    public const string NOT_FOUND = "Kiot_404";
    public const string MA_KIOT_REQUIRED = "Kiot_001";
    public const string MA_KIOT_DUPLICATE = "Kiot_002";
    public const string DIEN_TICH_INVALID = "Kiot_003";
    public const string TRANG_THAI_REQUIRED = "Kiot_004";
    public const string TRANG_THAI_INVALID = "Kiot_005";
    public const string KIOT_NOT_AVAILABLE = "Kiot_006";       // Kiot không ở trạng thái có thể thuê
}

public static class ThuongNhanMessage
{
    public const string NOT_FOUND = "ThuongNhan_404";
    public const string TEN_REQUIRED = "ThuongNhan_001";
    public const string SDT_REQUIRED = "ThuongNhan_002";
    public const string CCCD_REQUIRED = "ThuongNhan_003";
    public const string CCCD_DUPLICATE = "ThuongNhan_004";
}

public static class HopDongMessage
{
    public const string NOT_FOUND = "HopDong_404";
    public const string KIOT_REQUIRED = "HopDong_001";
    public const string THUONG_NHAN_REQUIRED = "HopDong_002";
    public const string NGAY_INVALID = "HopDong_003";          // NgayKetThuc phải sau NgayBatDau
    public const string GIA_THUE_INVALID = "HopDong_004";
    public const string LY_DO_REQUIRED = "HopDong_005";
    public const string KIOT_NOT_EXISTS = "HopDong_006";
    public const string KIOT_NOT_AVAILABLE = "HopDong_007";
    public const string ALREADY_TERMINATED = "HopDong_008";    // Hợp đồng đã chấm dứt
}

public static class HoaDonMessage
{
    public const string NOT_FOUND = "HoaDon_404";
    public const string HOP_DONG_REQUIRED = "HoaDon_001";
    public const string TONG_TIEN_INVALID = "HoaDon_002";
    public const string NGAY_PHAT_HANH_REQUIRED = "HoaDon_003";
    public const string HOP_DONG_NOT_FOUND = "HoaDon_004";
}

public static class ThanhToanMessage
{
    public const string NOT_FOUND = "ThanhToan_404";
    public const string HOA_DON_REQUIRED = "ThanhToan_001";
    public const string SO_TIEN_INVALID = "ThanhToan_002";
    public const string PHUONG_THUC_REQUIRED = "ThanhToan_003";
    public const string HOA_DON_DA_THANH_TOAN = "ThanhToan_004";  // Hóa đơn đã được thanh toán
}

public static class TaiSanMessage
{
    public const string NOT_FOUND = "TaiSan_404";
    public const string TEN_REQUIRED = "TaiSan_001";
    public const string LOAI_REQUIRED = "TaiSan_002";
    public const string GIA_TRI_INVALID = "TaiSan_003";
    public const string QR_NOT_FOUND = "TaiSan_404_QR";
}

public static class SuCoMessage
{
    public const string NOT_FOUND = "SuCo_404";
    public const string MUC_DO_REQUIRED = "SuCo_001";
    public const string MO_TA_REQUIRED = "SuCo_002";
    public const string TRANG_THAI_REQUIRED = "SuCo_003";
    public const string KET_QUA_REQUIRED = "SuCo_004";         // Bắt buộc nhập kết quả khi hoàn thành
}

public static class NguoiDungMessage
{
    public const string NOT_FOUND = "NguoiDung_404";
    public const string EXTERNAL_ID_REQUIRED = "NguoiDung_001";
    public const string EXTERNAL_ID_DUPLICATE = "NguoiDung_002";
    public const string TEN_REQUIRED = "NguoiDung_003";
    public const string VAI_TRO_REQUIRED = "NguoiDung_004";
}

public static class LichKiemTraMessage
{
    public const string NOT_FOUND = "LichKiemTra_404";
    public const string TIEU_DE_REQUIRED = "LichKiemTra_001";
    public const string LOAI_REQUIRED = "LichKiemTra_002";
    public const string NGAY_REQUIRED = "LichKiemTra_003";
}
