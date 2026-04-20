using Application.DTOs.HoaDon;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Hóa đơn — phát hành, theo dõi công nợ, quá hạn</summary>
public class HoaDonController : BaseController
{
    private readonly IHoaDonService _service;
    public HoaDonController(IHoaDonService service) => _service = service;

    /// <summary>Lấy danh sách hóa đơn, lọc theo hợp đồng, trạng thái, khoảng thời gian phát hành</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] HoaDonQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết hóa đơn theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy danh sách hóa đơn quá hạn chưa thanh toán</summary>
    [HttpGet("qua-han")]
    public async Task<IActionResult> GetQuaHan() =>
        Ok(await _service.GetQuaHanAsync());

    /// <summary>Tạo mới hóa đơn thủ công kèm chi tiết các khoản phí</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HoaDonRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Phát hành hóa đơn tự động theo tháng từ hợp đồng — tính phí thuê tự động</summary>
    [HttpPost("phat-hanh")]
    public async Task<IActionResult> PhatHanh([FromBody] PhatHanhHoaDonDTO request) =>
        Ok(await _service.PhatHanhAsync(request.HopDongId, request.Thang));

    /// <summary>Cập nhật hóa đơn</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HoaDonRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Xóa hóa đơn</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
