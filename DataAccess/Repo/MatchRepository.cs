using DataAccess.IRepo;
using Models.Models;

namespace DataAccess.Repo
{
    public class MatchRepository : BaseRepository<MatchQuestion>, IMatchRepository
    {
        public MatchRepository(AppDbContext context) : base(context) { }
    }
}
