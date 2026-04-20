using Application.DTOs.SuCo;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SuCoController : BaseController
{
    private readonly ISuCoService _service;
    public SuCoController(ISuCoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] SuCoQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> TiepNhan([FromBody] SuCoRequestDTO request) => Ok(await _service.TiepNhanAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SuCoRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpPatch("{id:guid}/trang-thai")]
    public async Task<IActionResult> CapNhatTrangThai(Guid id, [FromBody] CapNhatTrangThaiSuCoDTO request) => Ok(await _service.CapNhatTrangThaiAsync(id, request.TrangThai, request.KetQua));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
