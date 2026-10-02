using Application.DTOs.User;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Người dùng</summary>
public class UserController : BaseController
{
    private readonly IUserService _service;
    public UserController(IUserService service) => _service = service;

    /// <summary>Lấy danh sách người dùng có phân trang và tìm kiếm</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] UserQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết người dùng theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Tạo mới người dùng</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin người dùng</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa người dùng</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}