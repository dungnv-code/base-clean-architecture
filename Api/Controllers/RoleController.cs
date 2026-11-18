
using Application.IServices;
using Application.DTOs.Role;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

/// <summary>Quản lý vai trò</summary>

public class RoleController : BaseController
{
    private readonly IRoleService _service;
    public RoleController(IRoleService service) => _service = service;

    /// <summary>Lấy danh sách vai trò có phân trang và tìm kiếm</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] RoleQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết vai trò theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));
    
    /// <summary>Tạo mới vai trò</summary>
    /// <param name="request">Thông tin vai trò</param>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin vai trò</summary>
    /// <param name="id">ID vai trò</param> 
    /// <param name="request">Thông tin vai trò</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleDTO request) =>
        Ok(await _service.UpdateAsync(id, request));
    
    /// <summary>Xóa vai trò</summary>
    /// <param name="id">ID vai trò</param>
    /// <returns>Trả về kết quả xóa</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}