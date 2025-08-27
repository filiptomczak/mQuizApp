using DataAccess.IRepo;
using Models.Models;

namespace DataAccess.Repo
{
    public class SingleChoiceRepository : BaseRepository<SingleChoiceQuestion>, ISingleChoiceRepository
    {
        public SingleChoiceRepository(AppDbContext context) : base(context) { }
    }
}
