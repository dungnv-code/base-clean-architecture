using Application.DTOs.Cho;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Chợ</summary>
public class ChoController : BaseController
{
    private readonly IChoService _service;
    public ChoController(IChoService service) => _service = service;

    /// <summary>Lấy danh sách chợ có phân trang và tìm kiếm</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ChoQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết chợ theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tạo mới chợ</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ChoRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin chợ</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ChoRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa chợ</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
