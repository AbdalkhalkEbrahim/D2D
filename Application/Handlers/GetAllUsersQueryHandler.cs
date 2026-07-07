using Application.Queries;
using Application.Response;
using Domain.DTOs;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;

namespace Application.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<GetUserAndStatusCount>>
    {
        private readonly D2DContext _context;
        public GetAllUsersQueryHandler(D2DContext context)
        {
            _context = context; 
        }
        public async Task<Result<GetUserAndStatusCount>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            int AllStatusCount=0, ActiveStatusCount = 0, PendingStatusCount, SusbendingStatusCoount, AllUsersCount = 0, CustomerCount = 0, ProducerCount = 0;
           

            var users = _context.Users.Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.ProfileImageUrl, u.NumOfReports, u.NumOfCollaborations, u.UserType, u.JoinDate,u.IdentityStatus });

            var counts = users.GroupBy(u => u.UserType)
               .Select(u => new
               {
                   AllUsersCntt = u.Count(),
                   ProducerCnt = u.Count(p => p.UserType == UserType.Producer),
                   CustomerCnt = u.Count(c => c.UserType == UserType.Customer),
               }).FirstOrDefault();

            AllUsersCount = counts.AllUsersCntt;
            CustomerCount = counts.CustomerCnt;
            ProducerCount = counts.ProducerCnt;

            if (request.isCustomer)
               users= users.Where(u => u.UserType == UserType.Customer);
            else if (request.isProducer)
                users= users.Where(u => u.UserType == UserType.Producer);

            var status = users.GroupBy(u => u.IdentityStatus)
                .Select(u => new
                {
                    AllStatusCntt = u.Count(),
                    ActiveStatusCnt = u.Count(a => a.IdentityStatus == VerificationStatus.Approved),
                    PendingStatusCnt = u.Count(p => p.IdentityStatus == VerificationStatus.Pending),
                    SusbendedStatusCnt=u.Count(s=>s.IdentityStatus==VerificationStatus.Suspended)
                }).FirstOrDefault();

            AllStatusCount = status.AllStatusCntt;
            ActiveStatusCount = status.ActiveStatusCnt;
            PendingStatusCount=status.PendingStatusCnt;
            SusbendingStatusCoount = status.SusbendedStatusCnt;

            if(request.isActive)
                users=users.Where(u=>u.IdentityStatus==VerificationStatus.Approved);
            else if(request.isPending)
                users=users.Where(u=>u.IdentityStatus==VerificationStatus.Pending);
            else if(request.isSusbending)
                users=users.Where(u=>u.IdentityStatus!=VerificationStatus.Suspended);

            users = request.isNewst ? users.OrderByDescending(u => u.JoinDate).ThenBy(u => u.Id) : users.OrderBy(u => u.JoinDate).ThenBy(u => u.Id);

            //if (request.PageNum<=0 && request.PageSize <= 0)
            //{
            //    request.PageNum = 1;
            //    request.PageSize = 6;
            //}

            users = users.Skip((request.PageNum - 1) * request.PageSize).Take(request.PageSize);

            return
                    new GetUserAndStatusCount
                    {
                        SusbendStatusCount = SusbendingStatusCoount,
                        ActiveStatusCount = ActiveStatusCount,
                        PendingStatusCount = PendingStatusCount,
                        AllStatusesCount = AllStatusCount,
                        AllCustomersCount = CustomerCount,
                        AllProducersCount = ProducerCount,
                        AllUsersCount = AllUsersCount,

                        UsersResponse = users.Select
                                      (u => new GetAllUsersResponse
                                      {
                                          Id = u.Id,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName,
                                          Email = u.Email,
                                          JoinDate = u.JoinDate,
                                          NumOfCollations = u.NumOfCollaborations,
                                          NumOfReports = u.NumOfReports,
                                          Type = u.UserType.ToString(),
                                          Status = u.IdentityStatus.ToString(),
                                      }).ToList()
                    };

        }
    }
}
