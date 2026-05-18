
using Domain.Entities.Shared;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Infrastructure.Context
{
    public class D2DContext:IdentityDbContext<User>
    {

        public D2DContext() : base()
        {
           
        }
    }
}
