using ExtCore.Data.Abstractions;

namespace Infrastructure
{
    public class SqlHelper
    {
        private IStorageContext _context;

        public SqlHelper(IStorageContext context_)
        {
            _context = context_;
        }

        public void ExecuteSqlFile(string path_)

        {

        }

    }
}
