using Application.DTOs.TaiSan;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TaiSanController : BaseController
{
    private readonly ITaiSanService _service;
    public TaiSanController(ITaiSanService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TaiSanQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("qr/{qrCode}")]
    public async Task<IActionResult> GetByQRCode(string qrCode) => Ok(await _service.GetByQRCodeAsync(qrCode));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaiSanRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TaiSanRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
