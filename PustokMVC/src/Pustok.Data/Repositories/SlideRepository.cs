using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;

namespace Pustok.Data.Repositories
{
    public class SlideRepository : GenericRepository<Slide>, ISlideRepository
    {
        public SlideRepository(AppDbContext context) : base(context)
        {
        }
    }
}
