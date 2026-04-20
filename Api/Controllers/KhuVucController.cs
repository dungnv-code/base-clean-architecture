using Application.DTOs.KhuVuc;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Khu vực trong chợ</summary>
public class KhuVucController : BaseController
{
    private readonly IKhuVucService _service;
    public KhuVucController(IKhuVucService service) => _service = service;

    /// <summary>Lấy danh sách khu vực, lọc theo chợ và loại khu vực</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] KhuVucQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết khu vực theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tạo mới khu vực</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KhuVucRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin khu vực</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] KhuVucRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa khu vực</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
