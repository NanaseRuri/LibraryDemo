using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LibraryDemo.CustomPolicy
{
    public class MinimumEmploymentDurationHandler:AuthorizationHandler<EmploymentDurationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmploymentDurationRequirement requirement)
        {
            if (!context.User.HasClaim(c=>c.Type==ClaimTypes.Name&&c.Issuer==""))
            {
                return Task.FromResult(0);
            }

            var employmentDate =
                Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.Name && c.Issuer == "").Value);
            int numberOfMonths = (int)(DateTime.Today - employmentDate).TotalDays/30;
            if (numberOfMonths>requirement.MinimumMonths)
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
