using Application.DTOs.ThuongNhan;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Thương nhân — hồ sơ định danh, CCCD, giấy phép kinh doanh</summary>
public class ThuongNhanController : BaseController
{
    private readonly IThuongNhanService _service;
    public ThuongNhanController(IThuongNhanService service) => _service = service;

    /// <summary>Lấy danh sách thương nhân, tìm kiếm theo tên, SĐT, CCCD</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ThuongNhanQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết thương nhân theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tạo mới hồ sơ thương nhân — CCCD phải là duy nhất</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ThuongNhanRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin thương nhân</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ThuongNhanRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa thương nhân</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
