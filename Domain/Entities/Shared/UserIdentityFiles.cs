using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class UserIdentityFiles
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string FrontImageID { get; set; }
        public string BackImageID { get; set; }
        public string PersonalImage { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSamePerson {  get; set; }
        public decimal FaceSimilarity { get; set; }
        public string Confidence { get; set; }
        public List<string>? Observations { get; set; }

    }
}
