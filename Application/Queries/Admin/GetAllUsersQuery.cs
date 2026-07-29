using Application.Response;
using Domain.DTOs.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Admin
{
    public class GetAllUsersQuery : IRequest<Result<GetUserAndStatusCount>>
    {
        public bool isCustomer { get; set; } = false;
        public bool isProducer { get; set; } = false;
        public bool isAllUsers { get; set; } = true;
        public bool isAllStatus { get; set; } = true;
        public bool isActive { get; set; } = false;
        public bool isPending { get; set; } = false;
        public bool isSusbending { get; set; } = false;
        public bool isNewst { get; set; } = true;
        public bool ReportNumTextSearch { get; set; } = true;
        public string? UserAnnonNameTextSearch { get; set; }
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
