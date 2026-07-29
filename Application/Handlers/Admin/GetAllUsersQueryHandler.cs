using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;

namespace Application.Handlers.Admin
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
            int AllStatusCount = 0, ActiveStatusCount = 0, PendingStatusCount, SusbendingStatusCoount, AllUsersCount = 0, CustomerCount = 0, ProducerCount = 0;


            var users = _context.Users.Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.ProfileImageUrl, u.NumOfReports, u.NumOfCollaborations, u.UserType, u.JoinDate, u.IdentityStatus, u.AnonName,u.IsDeleted });

            if (request.UserAnnonNameTextSearch != null)
                users = users.Where(u => (u.FirstName + " " + u.LastName).Contains(request.UserAnnonNameTextSearch) || u.AnonName.Contains(request.UserAnnonNameTextSearch));


            var counts = users.GroupBy(_ => 1)
               .Select(u => new
               {
                   AllUsersCntt = u.Count(),
                   ProducerCnt = u.Count(p => p.UserType == UserType.Producer),
                   CustomerCnt = u.Count(c => c.UserType == UserType.Customer),
               }).FirstOrDefault();

            AllUsersCount = counts.AllUsersCntt;
            CustomerCount = counts.CustomerCnt;
            ProducerCount = counts.ProducerCnt;
            //AllUsersCount = users.Count();
            //CustomerCount = users.Count(u => u.UserType == UserType.Customer);
            //ProducerCount = users.Count(u => u.UserType == UserType.Producer);

            if (request.isCustomer)
                users = users.Where(u => u.UserType == UserType.Customer);
            else if (request.isProducer)
                users = users.Where(u => u.UserType == UserType.Producer);

            var status = users.GroupBy(_ => 1)
                .Select(u => new
                {
                    AllStatusCntt = u.Count(),
                    ActiveStatusCnt = u.Count(a => a.IdentityStatus == VerificationStatus.Approved),
                    PendingStatusCnt = u.Count(p => p.IdentityStatus == VerificationStatus.Pending),
                    SusbendedStatusCnt = u.Count(s => s.IdentityStatus == VerificationStatus.Suspended)
                }).FirstOrDefault();

            AllStatusCount = status.AllStatusCntt;
            ActiveStatusCount = status.ActiveStatusCnt;
            PendingStatusCount = status.PendingStatusCnt;
            SusbendingStatusCoount = status.SusbendedStatusCnt;
            //AllStatusCount = users.Count();
            //ActiveStatusCount = users.Count(u => u.IdentityStatus == VerificationStatus.Approved);
            //PendingStatusCount = users.Count(u => u.IdentityStatus == VerificationStatus.Pending);
            //SusbendingStatusCoount = users.Count(u => u.IdentityStatus == VerificationStatus.Suspended);

            if (request.isActive)
                users = users.Where(u => u.IdentityStatus == VerificationStatus.Approved&&!u.IsDeleted);
            else if (request.isPending)
                users = users.Where(u => u.IdentityStatus == VerificationStatus.Pending && !u.IsDeleted);
            else if (request.isSusbending)
                users = users.Where(u => u.IdentityStatus == VerificationStatus.Suspended || u.IsDeleted);


            users = request.isNewst ? users.OrderByDescending(u => u.JoinDate).ThenBy(u => u.Id) : users.OrderBy(u => u.JoinDate).ThenBy(u => u.Id);
            users = request.ReportNumTextSearch ? users.OrderByDescending(u => u.NumOfReports).ThenBy(u => u.Id) : users.OrderBy(u => u.NumOfReports).ThenBy(u => u.Id);

            if (request.PageNum <= 0)
                request.PageNum = 1;
            if (request.PageSize <= 0)
                request.PageSize = 6;


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
                                          Name = u.FirstName+" "+u.LastName,
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
