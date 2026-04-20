using Application.DTOs.ThanhToan;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Thanh toán — xử lý giao dịch, đối soát hóa đơn</summary>
public class ThanhToanController : BaseController
{
    private readonly IThanhToanService _service;
    public ThanhToanController(IThanhToanService service) => _service = service;

    /// <summary>Lấy danh sách thanh toán, lọc theo hóa đơn, phương thức, trạng thái, khoảng thời gian</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ThanhToanQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết thanh toán theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy tất cả giao dịch thanh toán của một hóa đơn</summary>
    [HttpGet("hoa-don/{hoaDonId:guid}")]
    public async Task<IActionResult> GetByHoaDon(Guid hoaDonId) =>
        Ok(await _service.GetByHoaDonAsync(hoaDonId));

    /// <summary>Xử lý thanh toán — tự động cập nhật trạng thái hóa đơn sang DaThanhToan</summary>
    [HttpPost]
    public async Task<IActionResult> XuLyThanhToan([FromBody] ThanhToanRequestDTO request) =>
        Ok(await _service.XuLyThanhToanAsync(request));

    /// <summary>Xóa giao dịch thanh toán</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
