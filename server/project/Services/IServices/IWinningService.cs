using project.Models;

namespace project.Services.IServices
{
    public interface IWinningService
    {
        Task<IEnumerable<Winning>> GetWinningAsync();

        Task<Winning?> GetByIdAsync(int id);
        Task DeleteWinningAsync(int id);
    }
}
