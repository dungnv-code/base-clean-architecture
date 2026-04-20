using Application.DTOs.Kiot;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Kiot — mã định danh, vị trí, trạng thái</summary>
public class KiotController : BaseController
{
    private readonly IKiotService _service;
    public KiotController(IKiotService service) => _service = service;

    /// <summary>Lấy danh sách kiot, lọc theo khu vực, trạng thái, loại kinh doanh</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] KiotQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết kiot theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy tất cả kiot thuộc một khu vực</summary>
    [HttpGet("khu-vuc/{khuVucId:guid}")]
    public async Task<IActionResult> GetByKhuVuc(Guid khuVucId) =>
        Ok(await _service.GetByKhuVucAsync(khuVucId));

    /// <summary>Lấy danh sách kiot sắp hết hạn hợp đồng (mặc định 30 ngày)</summary>
    [HttpGet("sap-het-han")]
    public async Task<IActionResult> GetSapHetHan([FromQuery] int soNgay = 30) =>
        Ok(await _service.GetSapHetHanAsync(soNgay));

    /// <summary>Tạo mới kiot</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KiotRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin kiot</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] KiotRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Cập nhật trạng thái kiot: Trong | DangChoThue | SapHetHan | DangBaoTri | DangCoc</summary>
    [HttpPatch("{id:guid}/trang-thai")]
    public async Task<IActionResult> CapNhatTrangThai(Guid id, [FromBody] string trangThai) =>
        Ok(await _service.CapNhatTrangThaiAsync(id, trangThai));

    /// <summary>Xóa kiot</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
