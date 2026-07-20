using Application.Commands.ChatFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application.Handlers.ChatFeature
{
    public class VerifyOfferOtpCommandHandler : IRequestHandler<VerifyOfferOtpCommand, Result<string>>
    {
        private readonly D2DContext _context;
        private readonly IOtpService _otpService;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        public VerifyOfferOtpCommandHandler(D2DContext context, IOtpService otpService, IChatService chatService, INotificationService notificationService) 
        {
            _context = context;
            _otpService = otpService;
            _chatService = chatService;
            _notificationService = notificationService;
        }
        public async Task<Result<string>> Handle(VerifyOfferOtpCommand request, CancellationToken cancellationToken)
        {
            var otp =await _context.Otps.FirstOrDefaultAsync(o => o.UserId == request.ProducerId && o.Code == request.Code);

            var isValid =_otpService.VerifyOtp(otp);

            if(!isValid.IsSuccess)
                return Result<string>.Failure(isValid.Error);

            var newActiveOfferLogIDs = await _context.ActiveOfferLogs
                .Select(a => new
                {
                    a.Chat.ID,
                    CName = a.Chat.Customer.AnonName,
                    PName = a.Chat.Customer.AnonName,
                    a.Chat.ProducerID,
                    a.Chat.CustomerID,
                    CustomTotalAmount = a.CustomerCustomOffer.ProducerCustomerOffer.Price,
                    PublishedTotalAmount = a.CustomerPublishedOffer.ProducerCustomerOffers.FirstOrDefault(c => c.ProducerID == a.Chat.ProducerID).Price,
                    CustomDeposit = a.CustomerCustomOffer.ProducerCustomerOffer.Diposit,
                    PublishedDeposit = a.CustomerPublishedOffer.ProducerCustomerOffers.FirstOrDefault(c => c.ProducerID == a.Chat.ProducerID).Diposit,
                    OfferId = a.CustomerPublishedOffer.ID,
                    CEmail = a.Chat.Customer.Email,
                    PEmail = a.Chat.Producer.Email,
                    CustomerPublishedOfferId= a.PublishedOfferID,
                    CustomOfferId=a.CustomOfferID,
                    ProducerPublishedOfferId=a.CustomerPublishedOffer.ProducerCustomerOffers.FirstOrDefault(p=>p.OfferStatus== OfferStatus.Accepted).ID,
                    ProducerCustomOfferId = a.CustomerCustomOffer.ProducerCustomerOfferID

                })
                .Where(c => c.ProducerID == request.ProducerId && c.CustomerID == request.CustomerId &&
                c.CustomerID == request.CustomerId)
                .Select(a => new { a.ID, a.OfferId, a.CEmail, a.PEmail,a.CustomTotalAmount,a.PublishedTotalAmount,a.CustomDeposit,a.PublishedDeposit,
                a.ProducerCustomOfferId,a.ProducerPublishedOfferId,a.CustomerPublishedOfferId,a.CustomOfferId})
                .FirstOrDefaultAsync();

            if (newActiveOfferLogIDs == null)
                return Result<string>.Failure(Messages.NotFound.WithTarget("ActiveOfferLog"));


            var activeOfferLog = new ActiveOfferLogs
            {
                Step = ActiveOfferStatus.Completed.ToString(),
                Notes = "Offer completed successfully",
                ChatID = newActiveOfferLogIDs.ID,
                PublishedOfferID = newActiveOfferLogIDs.OfferId
            };


            decimal deposit = 0m, total = 0m;
            if (newActiveOfferLogIDs.PublishedDeposit == null)
            {
                deposit = newActiveOfferLogIDs.CustomDeposit;
                total = newActiveOfferLogIDs.CustomTotalAmount - deposit;
            }
            else
            {
                deposit = newActiveOfferLogIDs.PublishedDeposit;
                total = newActiveOfferLogIDs.PublishedDeposit - deposit;
            }

            
            await _context.Users
            .Where(u => u.Id == request.ProducerId)
            .ExecuteUpdateAsync(s => s.SetProperty(
                u => u.Balance,
            u => u.Balance + (total-(total*0.15m))
            ), cancellationToken);
            


                var cNotification = new Notification
            {
                NotificationsType = NotificationsType.ActveOfferStatuesChanged,
                Title = "Offer completed",
                Content = $"Your offer with {newActiveOfferLogIDs.CName} has been completed successfully, add a review",
                RefrenceUrl = $"/reviews/add-review/{request.ProducerId}",
                UserID = request.CustomerId
            };
            var pNotification = new Notification
            {
                NotificationsType = NotificationsType.ActveOfferStatuesChanged,
                Title = "Offer completed",
                Content = $"Your offer with {newActiveOfferLogIDs.PName} has been completed successfully, check your balance",
                RefrenceUrl = $"/accountSettings/get-profile/{request.ProducerId}",
                UserID = request.ProducerId
            };

            await _notificationService.CompleteDeal(cNotification, pNotification);
            BackgroundJob.Enqueue<IEmailService>(emailService =>
                emailService.SendEmailAsync(
                    newActiveOfferLogIDs.CEmail,
                    "Offer completed",
                    $"Your offer with {request.CustomerId} has been completed successfully, check your balance here https://design-to-dress.vercel.app/get-profile/{request.ProducerId}"
                ));

            BackgroundJob.Enqueue<IEmailService>(emailService =>
                emailService.SendEmailAsync(
                    newActiveOfferLogIDs.PEmail,
                    "Offer completed",
                    $"Your offer with {request.ProducerId} has been completed successfully, check your balance here https://design-to-dress.vercel.app/add-review/{request.ProducerId}"
                ));





            if (newActiveOfferLogIDs.CustomOfferId == null)
            {
                await _context.ProducerCustomerOffers.Where(o => o.ID == newActiveOfferLogIDs.ProducerPublishedOfferId)
             .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.OfferStatus, p => OfferStatus.Completed));

                await _context.CustomerPublishedOffers.Where(o => o.ID == newActiveOfferLogIDs.CustomerPublishedOfferId)
                   .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.CustomerOfferStatus, p => OfferStatus.Completed));
            }
            else
            {
                await _context.ProducerCustomerOffers.Where(o => o.ID == newActiveOfferLogIDs.ProducerCustomOfferId)
             .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.OfferStatus, p => OfferStatus.Completed));

                await _context.CustomerCustomOffers.Where(o => o.ID == newActiveOfferLogIDs.CustomOfferId)
                   .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.CustomerOfferStatus, p => OfferStatus.Completed));
            }


            otp.IsUsed = true;
            _context.Attach(otp);
            _context.Entry(otp).State = EntityState.Modified;

            var updatedChat = new Chat { ID = newActiveOfferLogIDs.ID, IsClosed = true, UpdatedAt = DateTime.UtcNow };
            _context.Attach(updatedChat);
            _context.Entry(updatedChat).Property(ch => ch.IsClosed).IsModified = true; 
            _context.Entry(updatedChat).Property(ch => ch.UpdatedAt).IsModified = true;


            await _context.AddAsync(activeOfferLog);
            await _context.SaveChangesAsync(cancellationToken);

            await _context.ProducerCustomerOffers.Where(o => o.ID == newActiveOfferLogIDs.ProducerOfferId)
                .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.OfferStatus, p => OfferStatus.Completed));

            await _context.CustomerPublishedOffers.Where(o => o.ID == newActiveOfferLogIDs.CustomerOfferId)
               .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.CustomerOfferStatus, p => OfferStatus.Completed));

            return request.ProducerId;
        }
    }
}
