using ProjectClassLibrary.Repository.BaseClasses;
using ProjectClassLibrary.Repository.Utils;
using ProjectClassLibrary.Model;

namespace ProjectClassLibrary.Repository.DerivedClass
{
    public class CaseSqlRepository:BaseSqlRepository<CaseEntity>
    {
       
        public CaseSqlRepository()
        {
        }

        public CaseSqlRepository(IConnectionInfo co): base(co)
        {
        }
        
    }
}
