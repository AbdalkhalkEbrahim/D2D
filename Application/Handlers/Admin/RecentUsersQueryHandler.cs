using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class RecentUsersQueryHandler : IRequestHandler<RecentUsersQuery, Result<List<RecentUsersResponse>>>
    {
        private readonly D2DContext _context;
        public RecentUsersQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<RecentUsersResponse>>> Handle(RecentUsersQuery request, CancellationToken cancellationToken)
        {
            return _context.Users.Select(u => new RecentUsersResponse
            {
                Id = u.Id,
                Name=u.FirstName+" "+u.LastName,
                ProfileImage=u.ProfileImageUrl,
                CreatedAt= u.JoinDate,
                Status = u.IdentityStatus.ToString(),
                Type = u.UserType.ToString()
            }).OrderByDescending(u=>u.CreatedAt).ThenBy(u=>u.Id).Take(3).ToList();  
        }
    }
}
