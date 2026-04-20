#nullable disable

using Application.Common;
using Application.DTOs.NguoiDung;
using Domain.Entities;

namespace Application.IRepositories;

public interface INguoiDungRepository : IRepository<NguoiDung>
{
    Task<PagedResult<NguoiDung>> GetListAsync(NguoiDungQueryDTO query);
    Task<NguoiDung> GetByExternalIdAsync(string externalId);
}
