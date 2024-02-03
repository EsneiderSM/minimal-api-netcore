using MinimalAPIMovies.Entities;

namespace MinimalAPIMovies.Repositories
{
    public interface IRepositoryGender
    {
        Task<List<Gender>> GetAll();
        Task<Gender?> GetGenderById(int id);
        Task<int> Create(Gender gender);
        Task<bool> Exists(int id);
        Task Update(Gender gender);
        Task Delete(int id);
    }
}
