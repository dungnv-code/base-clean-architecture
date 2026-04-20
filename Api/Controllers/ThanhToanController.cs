using Application.DTOs.ThanhToan;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ThanhToanController : BaseController
{
    private readonly IThanhToanService _service;
    public ThanhToanController(IThanhToanService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ThanhToanQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("hoa-don/{hoaDonId:guid}")]
    public async Task<IActionResult> GetByHoaDon(Guid hoaDonId) => Ok(await _service.GetByHoaDonAsync(hoaDonId));

    [HttpPost]
    public async Task<IActionResult> XuLyThanhToan([FromBody] ThanhToanRequestDTO request) => Ok(await _service.XuLyThanhToanAsync(request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
