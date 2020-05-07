using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace GraphExplorer.Utilities
{
    public static class Utilities
    {
        public static string GetCurrentUserTenant(IPrincipal user)
        {
            return (user.Identity as ClaimsIdentity)?.Claims
                .Where(_ => _.Type == "http://schemas.microsoft.com/identity/claims/tenantid")
                .ToList()
                .FirstOrDefault()
                ?.Value
                .ToString();
        }

        public static string GetCurrentUserName(IPrincipal user)
        {
            return (user.Identity as ClaimsIdentity)?.Claims
                .Where(_ => _.Type == "name")
                .ToList()
                .FirstOrDefault()
                ?.Value
                .ToString();
        }
    }
}