using Application.DTOs.NguoiDung;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Người dùng hệ thống — nhân sự ban quản lý</summary>
public class NguoiDungController : BaseController
{
    private readonly INguoiDungService _service;
    public NguoiDungController(INguoiDungService service) => _service = service;

    /// <summary>Lấy danh sách người dùng, lọc theo vai trò và trạng thái</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] NguoiDungQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết người dùng theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy người dùng theo ExternalId (dùng khi tích hợp hệ thống xác thực bên ngoài)</summary>
    [HttpGet("external/{externalId}")]
    public async Task<IActionResult> GetByExternalId(string externalId) =>
        Ok(await _service.GetByExternalIdAsync(externalId));

    /// <summary>Tạo mới người dùng — ExternalId phải là duy nhất</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NguoiDungRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin người dùng</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] NguoiDungRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa người dùng</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
