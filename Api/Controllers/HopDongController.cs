using Application.DTOs.HopDong;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>Quản lý Hợp đồng thuê kiot</summary>
public class HopDongController : BaseController
{
    private readonly IHopDongService _service;
    public HopDongController(IHopDongService service) => _service = service;

    /// <summary>Lấy danh sách hợp đồng, lọc theo kiot, thương nhân, trạng thái, khoảng thời gian</summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] HopDongQueryDTO query) =>
        Ok(await _service.GetListAsync(query));

    /// <summary>Lấy chi tiết hợp đồng theo ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Lấy danh sách hợp đồng sắp hết hạn (mặc định 30 ngày)</summary>
    [HttpGet("sap-het-han")]
    public async Task<IActionResult> GetSapHetHan([FromQuery] int soNgay = 30) =>
        Ok(await _service.GetSapHetHanAsync(soNgay));

    /// <summary>Lấy tất cả hợp đồng của một kiot</summary>
    [HttpGet("kiot/{kiotId:guid}")]
    public async Task<IActionResult> GetByKiot(Guid kiotId) =>
        Ok(await _service.GetByKiotAsync(kiotId));

    /// <summary>Tạo mới hợp đồng — tự động cập nhật trạng thái kiot sang DangChoThue</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HopDongRequestDTO request) =>
        Ok(await _service.CreateAsync(request));

    /// <summary>Cập nhật thông tin hợp đồng</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HopDongRequestDTO request) =>
        Ok(await _service.UpdateAsync(id, request));

    /// <summary>Chấm dứt hợp đồng — tự động trả kiot về trạng thái Trong</summary>
    [HttpPatch("{id:guid}/cham-dut")]
    public async Task<IActionResult> ChamDut(Guid id, [FromBody] ChamDutHopDongDTO request) =>
        Ok(await _service.ChamDutAsync(id, request.LyDo));

    /// <summary>Xóa hợp đồng</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _service.DeleteAsync(id));
}
