using Application.DTOs.SuCo;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Sự cố — tiếp nhận, phân loại, điều phối xử lý</summary>
public class SuCoController : BaseController
{
    private readonly ISuCoService _service;
    public SuCoController(ISuCoService service) => _service = service;

    /// <summary>Lấy danh sách sự cố, lọc theo kiot, mức độ, trạng thái</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] SuCoQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết sự cố theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tiếp nhận sự cố mới — trạng thái khởi tạo: MoiTiepNhan</summary>
    [HttpPost]
    public async Task<IActionResult> TiepNhan([FromBody] SuCoRequestDTO request) =>
        Ok(await _service.TiepNhanAsync(request));

    /// <summary>Cập nhật thông tin sự cố</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SuCoRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Cập nhật trạng thái xử lý sự cố: MoiTiepNhan | DangXuLy | HoanThanh | DaDong</summary>
    [HttpPatch("{id:guid}/trang-thai")]
    public async Task<IActionResult> CapNhatTrangThai(Guid id, [FromBody] CapNhatTrangThaiSuCoDTO request) =>
        Ok(await _service.CapNhatTrangThaiAsync(id, request.TrangThai, request.KetQua));

    /// <summary>Xóa sự cố</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
