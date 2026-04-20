using Application.DTOs.HoaDon;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class HoaDonController : BaseController
{
    private readonly IHoaDonService _service;
    public HoaDonController(IHoaDonService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] HoaDonQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("qua-han")]
    public async Task<IActionResult> GetQuaHan() => Ok(await _service.GetQuaHanAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HoaDonRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPost("phat-hanh")]
    public async Task<IActionResult> PhatHanh([FromBody] PhatHanhHoaDonDTO request) => Ok(await _service.PhatHanhAsync(request.HopDongId, request.Thang));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HoaDonRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
