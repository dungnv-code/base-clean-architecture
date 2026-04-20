#nullable disable

using Application.Common;
using Application.DTOs.TaiSan;
using Domain.Entities;

namespace Application.IRepositories;

public interface ITaiSanRepository : IRepository<TaiSan>
{
    Task<PagedResult<TaiSan>> GetListAsync(TaiSanQueryDTO query);
    Task<TaiSan> GetByQRCodeAsync(string qrCode);
}
