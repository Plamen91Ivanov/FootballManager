using System.Threading.Tasks;

namespace FootballManager.Services.Data
{
    public interface IAddGames
    {
        Task<int> AddAsync();
    }
}