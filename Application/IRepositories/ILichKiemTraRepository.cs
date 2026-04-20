#nullable disable

using Application.Common;
using Application.DTOs.LichKiemTra;
using Domain.Entities;

namespace Application.IRepositories;

public interface ILichKiemTraRepository : IRepository<LichKiemTra>
{
    Task<PagedResult<LichKiemTra>> GetListAsync(LichKiemTraQueryDTO query);
    Task<List<LichKiemTra>> GetSapDenAsync(int soNgay);
}
