using Microsoft.EntityFrameworkCore;
using MinimalAPIMovies.Entities;

namespace MinimalAPIMovies.Repositories
{
    public class RepositoryGender : IRepositoryGender
    {
        private readonly ApplicationDbContext context;

        public RepositoryGender(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Gender.AnyAsync(gender => gender.Id == id);
        }

        public async Task<List<Gender>> GetAll()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<Gender?> GetGenderById(int id)
        {
            return await context.Gender.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Create(Gender gender)
        {
            context.Add(gender);
            await context.SaveChangesAsync();
            return gender.Id;
        }

        public async Task Update(Gender gender)
        {
            context.Update(gender);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Gender.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
