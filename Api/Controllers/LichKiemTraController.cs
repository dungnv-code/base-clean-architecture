using Application.DTOs.LichKiemTra;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class LichKiemTraController : BaseController
{
    private readonly ILichKiemTraService _service;
    public LichKiemTraController(ILichKiemTraService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] LichKiemTraQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("sap-den")]
    public async Task<IActionResult> GetSapDen([FromQuery] int soNgay = 7) => Ok(await _service.GetSapDenAsync(soNgay));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LichKiemTraRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LichKiemTraRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
