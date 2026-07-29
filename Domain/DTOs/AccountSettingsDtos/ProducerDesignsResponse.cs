using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AccountSettingsDtos
{
    public class ProducerDesignsResponse
    {
        public string AnonName { get; set; }

        public List<GalleryResponse> GalleryResponse { get; set; }
    }
}
