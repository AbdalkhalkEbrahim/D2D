using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        public Task SendPuplishedDesignNotificationAsync(Guid customerPOfferId);
        public Task SendProducerOfferNotification(Guid clientId, Guid producerOfferId);
        public Task DeclineProducerOfferNotification(string producerId, string offerName);
    }
}
