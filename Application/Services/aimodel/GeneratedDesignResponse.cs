using Domain.Entities.Chats.AiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.zena
{
    public class GeneratedDesignResponse
    {
        public Guid GeneratedDesignId { get; set; }
        public string CustomerId { get; set; }
        public ModelChat ModelChat { get; set; }
        public DesignState DesignState { get; set; }
        
    }
}
