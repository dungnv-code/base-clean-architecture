using Application.DTOs.KhuVuc;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class KhuVucController : BaseController
{
    private readonly IKhuVucService _service;
    public KhuVucController(IKhuVucService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] KhuVucQueryDTO query) => Ok(await _service.GetListAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KhuVucRequestDTO request) => Ok(await _service.CreateAsync(request));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] KhuVucRequestDTO request) => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
