using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectClassLibrary.Repository.BaseClasses;
using ProjectClassLibrary.Repository.DerivedClass;
using ProjectClassLibrary.Model;
using ProjectClassLibrary.Repository.Interface;
using ProjectClassLibrary.Repository.Utils;
namespace ConsoleTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            IRepository<CaseEntity> c1 = new CaseSqlRepository()
            {
                ConnectionDetails =new ConnectionInfo()
                {
                    servername = "iris.arvixe.com", databasename = "Heartbeat", username = "hbuser", password = "heartbeat@8681"
                }
            };
            List<CaseEntity> allCases=c1.Get();

            foreach (CaseEntity c in allCases)
            {
                Console.WriteLine(c.Id);
                Console.WriteLine(c.Name);
                Console.WriteLine(c.CaseDescription);
                Console.WriteLine(c.StartDate);
                Console.WriteLine(c.Active);
                Console.WriteLine(c.CaseType);
            }

            //c1.Update(new CaseEntity(){Id=2,Name = "Updated Case for me",StartDate = new DateTime(2014,10,6),CaseDescription = "horrraaaah",CaseType = "Hola",Active=false});
            c1.Delete(new CaseEntity() {Id = 2});
        }

        
    }
}
