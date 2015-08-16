using System;
using ProjectClassLibrary.Model.CustomAttributes;

namespace ProjectClassLibrary.Model
{
    public  class CaseEntity:IEntity
    {
        [PrimaryKey][IgnoreField]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string CaseType { get; set; }
        public string CaseDescription { get; set; }
        public bool Active { get; set; }
        [IgnoreField]
        public string ErrorMessage { get; set; }
        
    }
}
