using Application.DTOs.NguoiDung;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class NguoiDungController : BaseController
{
    private readonly INguoiDungService _service;
    public NguoiDungController(INguoiDungService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] NguoiDungQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("external/{externalId}")]
    public async Task<IActionResult> GetByExternalId(string externalId) => Ok(await _service.GetByExternalIdAsync(externalId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NguoiDungRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] NguoiDungRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
