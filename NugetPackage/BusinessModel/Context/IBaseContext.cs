using Microsoft.EntityFrameworkCore;

namespace BusinessModel.Context
{
    public interface IBaseContext<T> where T : DbContext
    {
        T GetDbContext();
    }
}
