using Application.DTOs.LichKiemTra;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Lịch kiểm tra — an ninh, PCCC, vệ sinh định kỳ</summary>
public class LichKiemTraController : BaseController
{
    private readonly ILichKiemTraService _service;
    public LichKiemTraController(ILichKiemTraService service) => _service = service;

    /// <summary>Lấy danh sách lịch kiểm tra, lọc theo khu vực, loại kiểm tra, trạng thái, khoảng thời gian</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] LichKiemTraQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết lịch kiểm tra theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy danh sách lịch kiểm tra sắp diễn ra (mặc định 7 ngày tới)</summary>
    [HttpGet("sap-den")]
    public async Task<IActionResult> GetSapDen([FromQuery] int soNgay = 7) =>
        Ok(await _service.GetSapDenAsync(soNgay));

    /// <summary>Tạo mới lịch kiểm tra</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LichKiemTraRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật lịch kiểm tra</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LichKiemTraRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa lịch kiểm tra</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
