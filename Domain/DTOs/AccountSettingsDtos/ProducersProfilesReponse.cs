using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AccountSettingsDtos
{
    public class ProducersProfilesReponse
    {
        public string ProducerId { get; set; }
        public string AnonName  { get; set; }
        public string ProfileImage {  get; set; }
        public int NumberOfDesigns {  get; set; }
        public int NumberOfCollaborations { get; set; }
        public int Rate {  get; set; }
    }
}
