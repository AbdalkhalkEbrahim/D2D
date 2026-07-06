using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        public Task SendMessage(Notification notification, Notification? limit);
        public Task SendPuplishedDesignNotificationAsync(Guid customerPOfferId);
        public Task SendProducerOfferNotification(string clientId, Guid producerOfferId);
        public Task DeclineProducerOfferNotification(string producerId, string offerName);
        public Task ChangeStatus(Notification cNotification, Notification pNotification);
        public Task SendRequest(Notification cNotification, Notification pNotification);
        public Task CompleteDeal(Notification cNotification, Notification pNotification);
        


    }
}
