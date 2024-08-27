using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext context) : base(context)
        {
        }
    }
}
