using Application.DTOs.TaiSan;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Tài sản — danh mục, QR code, vị trí, bảo hành</summary>
public class TaiSanController : BaseController
{
    private readonly ITaiSanService _service;
    public TaiSanController(ITaiSanService service) => _service = service;

    /// <summary>Lấy danh sách tài sản, lọc theo khu vực, kiot, loại, trạng thái</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TaiSanQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết tài sản theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tra cứu tài sản bằng mã QR code</summary>
    [HttpGet("qr/{qrCode}")]
    public async Task<IActionResult> GetByQRCode(string qrCode) =>
        Ok(await _service.GetByQRCodeAsync(qrCode));

    /// <summary>Tạo mới tài sản</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaiSanRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin tài sản</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TaiSanRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa tài sản</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
