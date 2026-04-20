using Application.DTOs.Kiot;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class KiotController : BaseController
{
    private readonly IKiotService _service;
    public KiotController(IKiotService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] KiotQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("khu-vuc/{khuVucId:guid}")]
    public async Task<IActionResult> GetByKhuVuc(Guid khuVucId) => Ok(await _service.GetByKhuVucAsync(khuVucId));

    [HttpGet("sap-het-han")]
    public async Task<IActionResult> GetSapHetHan([FromQuery] int soNgay = 30) => Ok(await _service.GetSapHetHanAsync(soNgay));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KiotRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] KiotRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpPatch("{id:guid}/trang-thai")]
    public async Task<IActionResult> CapNhatTrangThai(Guid id, [FromBody] string trangThai) => Ok(await _service.CapNhatTrangThaiAsync(id, trangThai));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
