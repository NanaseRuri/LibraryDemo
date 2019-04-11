using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace LibraryDemo.CustomPolicy
{
    public class EmploymentDurationRequirement:IAuthorizationRequirement
    {
        public int MinimumMonths { get; set; }

        public EmploymentDurationRequirement(int minimumMonths)
        {
            MinimumMonths = minimumMonths;
        }
    }
}
