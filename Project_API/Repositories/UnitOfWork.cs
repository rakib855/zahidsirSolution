using Project_API.Models;
using Project_API.Repositories.Interfaces;

namespace Project_API.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        TeacherDbContext db;
        public UnitOfWork(TeacherDbContext db)
        {
            this.db = db;
        }
        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        public IGenericRepo<T> GetRepo<T>() where T : class, new()
        {
            return new GenericRepo<T>(this.db);
        }
    }
}
