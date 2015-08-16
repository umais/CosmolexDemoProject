using System;
using System.Collections.Generic;
using System.Web.Http;
using ProjectClassLibrary.Repository.Interface;
using ProjectClassLibrary.Model;
using ProjectClassLibrary.Repository.DerivedClass;
using SampleWeb.Models;
namespace SampleWeb.Controllers
{
    public class CaseEntityController : ApiController
    {
        readonly IRepository<CaseEntity> _repo = new CaseSqlRepository();
       

       
        public List<CaseEntity> PostAll([FromBody]CaseViewModel c,string id)
        {
            var lst = new List<CaseEntity>();
            try
            {
                _repo.ConnectionDetails = c.ConnectionDetails;
                switch (id)
                {
                    case "Add":
                        _repo.Add(c.Model);
                        break;
                    case "Update":
                        _repo.Update(c.Model);
                        break;
                    case "Delete":
                        _repo.Delete(c.Model);
                        break;
                    default:
                        return _repo.Get();

                }
                lst=_repo.Get();
            }
            catch (Exception ex)
            {
                lst.Add(new CaseEntity(){ErrorMessage = ex.Message});
            }

            return lst;
        }

    }
}
