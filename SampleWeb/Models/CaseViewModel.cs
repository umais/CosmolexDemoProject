using ProjectClassLibrary.Model;
using ProjectClassLibrary.Repository.Utils;
namespace SampleWeb.Models
{
    public class CaseViewModel
    {
       public ConnectionInfo ConnectionDetails { get; set; }
        public CaseEntity Model { get; set; }
    }
}