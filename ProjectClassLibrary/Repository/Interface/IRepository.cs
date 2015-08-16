using System.Collections.Generic;
using ProjectClassLibrary.Repository.Utils;

namespace ProjectClassLibrary.Repository.Interface
{
   public  interface IRepository<T>
    {
         IConnectionInfo ConnectionDetails { get; set; }

        string ExceptionMessage { get; set; }
        List<T> Get();
        void Add(T t);
        T Update(T t);
        void Delete(T t);
    }
}
