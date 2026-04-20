using Application.DTOs.HopDong;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class HopDongController : BaseController
{
    private readonly IHopDongService _service;
    public HopDongController(IHopDongService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] HopDongQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("sap-het-han")]
    public async Task<IActionResult> GetSapHetHan([FromQuery] int soNgay = 30) => Ok(await _service.GetSapHetHanAsync(soNgay));

    [HttpGet("kiot/{kiotId:guid}")]
    public async Task<IActionResult> GetByKiot(Guid kiotId) => Ok(await _service.GetByKiotAsync(kiotId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HopDongRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HopDongRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpPatch("{id:guid}/cham-dut")]
    public async Task<IActionResult> ChamDut(Guid id, [FromBody] ChamDutHopDongDTO request) => Ok(await _service.ChamDutAsync(id, request.LyDo));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
