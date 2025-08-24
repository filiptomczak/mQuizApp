using DataAccess.IRepo;
using Models.Models;

namespace DataAccess.Repo
{
    public class OpenRepository : BaseRepository<OpenQuestion>, IOpenRepository
    {
        public OpenRepository(AppDbContext context) : base(context) { }
    }
}
