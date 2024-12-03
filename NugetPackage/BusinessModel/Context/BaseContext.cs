using Microsoft.EntityFrameworkCore;

namespace BusinessModel.Context
{
    public class BaseContext<T>: IBaseContext<T> where T : DbContext
    {
        protected readonly T _context;

        public BaseContext(T context)
        {
            _context = context;
        }
        public T GetDbContext()
        {
            return _context;
        }
    }
}
